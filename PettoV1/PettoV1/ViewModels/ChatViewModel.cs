using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PettoV1.Services;
using SharedResources.Data;
using SharedResources.Models;
using System.Collections.ObjectModel;

namespace PettoV1.ViewModels
{
    public partial class ChatViewModel : ObservableObject
    {
        private readonly DataContext _dataContext;
        private readonly APIService  _apiService;

        // Máximo de mensajes anteriores que se envían como contexto a Gemini
        private const int MaxHistorialContexto = 10;

        [ObservableProperty] private ObservableCollection<MensajeModel> _mensajes = new();
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PuedeEnviar))]
        private string _mensajeActual = string.Empty;
        [ObservableProperty] private string _fechaHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

        /// <summary>Indica que la IA está generando una respuesta (muestra indicador de espera).</summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PuedeEnviar))]
        private bool _estaRespondiendo;

        public bool PuedeEnviar => !string.IsNullOrWhiteSpace(MensajeActual) && !EstaRespondiendo;

        public ChatViewModel(DataContext dataContext, APIService apiService)
        {
            _dataContext = dataContext;
            _apiService  = apiService;
        }

        public async Task InicializarAsync()
        {
            int usuarioId = Preferences.Get("UsuarioId", 0);

            Mensajes.Clear();
            var historial = await _dataContext.Mensajes
                .AsNoTracking()
                .Where(m => m.UsuarioId == usuarioId)
                .OrderBy(m => m.FechaHora)
                .ToListAsync();

            foreach (var m in historial) Mensajes.Add(m);
        }

        [RelayCommand]
        public async Task EnviarMensaje()
        {
            if (!PuedeEnviar) return;

            int    usuarioId  = Preferences.Get("UsuarioId", 0);
            string textoEnv   = MensajeActual.Trim();
            MensajeActual     = string.Empty;

            // 1. Guardar y mostrar el mensaje del usuario
            var mensajeUsuario = new MensajeModel
            {
                Contenido    = textoEnv,
                EsRespuestaIA = false,
                FechaHora    = DateTime.Now,
                UsuarioId    = usuarioId
            };
            await _dataContext.Mensajes.AddAsync(mensajeUsuario);
            await _dataContext.SaveChangesAsync();
            Mensajes.Add(mensajeUsuario);

            // 2. Construir historial de contexto (últimos N mensajes)
            var historialContexto = Mensajes
                .TakeLast(MaxHistorialContexto)
                .Where(m => m != mensajeUsuario)   // Excluir el que acabamos de agregar
                .Select(m => new MensajeHistorial(m.EsRespuestaIA, m.Contenido))
                .ToList();

            // 3. Llamar a Gemini
            EstaRespondiendo = true;
            string respuestaTexto;
            try
            {
                respuestaTexto = await _apiService.EnviarMensajeAsync(textoEnv, historialContexto);
            }
            finally
            {
                EstaRespondiendo = false;
            }

            // 4. Guardar y mostrar la respuesta de la IA
            var mensajeIA = new MensajeModel
            {
                Contenido    = respuestaTexto,
                EsRespuestaIA = true,
                FechaHora    = DateTime.Now,
                UsuarioId    = usuarioId
            };
            await _dataContext.Mensajes.AddAsync(mensajeIA);
            await _dataContext.SaveChangesAsync();
            Mensajes.Add(mensajeIA);
        }

        [RelayCommand]
        public void AbrirMenu() => Shell.Current.FlyoutIsPresented = true;

        [RelayCommand]
        public async Task IrAPerfil() => await Shell.Current.GoToAsync("Perfil");

        [RelayCommand]
        public async Task AbrirMenuChat()
        {
            string accion = await Shell.Current.DisplayActionSheet(
                "Opciones", "Cancelar", null,
                "🗑️ Limpiar chat");

            if (accion == "🗑️ Limpiar chat")
                await LimpiarChatAsync();
        }

        private async Task LimpiarChatAsync()
        {
            bool confirmar = await Shell.Current.DisplayAlert(
                "Limpiar chat",
                "¿Eliminar todos los mensajes? Esta acción no se puede deshacer.",
                "Eliminar", "Cancelar");

            if (!confirmar) return;

            int usuarioId = Preferences.Get("UsuarioId", 0);
            var mensajes = await _dataContext.Mensajes
                .Where(m => m.UsuarioId == usuarioId)
                .ToListAsync();

            _dataContext.Mensajes.RemoveRange(mensajes);
            await _dataContext.SaveChangesAsync();
            Mensajes.Clear();
        }
    }
}
