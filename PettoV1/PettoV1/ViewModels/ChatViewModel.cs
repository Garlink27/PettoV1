using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using SharedResources.Data;
using SharedResources.Models;
using System.Collections.ObjectModel;

namespace PettoV1.ViewModels
{
    public partial class ChatViewModel : ObservableObject
    {
        private readonly DataContext _dataContext;

        [ObservableProperty] private ObservableCollection<MensajeModel> _mensajes = new();
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PuedeEnviar))]
        private string _mensajeActual = string.Empty;
        [ObservableProperty] private string _fechaHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

        public bool PuedeEnviar => !string.IsNullOrWhiteSpace(MensajeActual);

        public ChatViewModel(DataContext dataContext)
        {
            _dataContext = dataContext;
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

            int usuarioId = Preferences.Get("UsuarioId", 0);

            var mensaje = new MensajeModel
            {
                Contenido = MensajeActual,
                EsRespuestaIA = false,
                FechaHora = DateTime.Now,
                UsuarioId = usuarioId
            };

            await _dataContext.Mensajes.AddAsync(mensaje);
            await _dataContext.SaveChangesAsync();
            Mensajes.Add(mensaje);

            string texto = MensajeActual;
            MensajeActual = string.Empty;

            await Task.Delay(600);

            var respuestaIA = new MensajeModel
            {
                Contenido = $"Recibí tu mensaje: \"{texto}\". Próximamente tendrás respuestas inteligentes.",
                EsRespuestaIA = true,
                FechaHora = DateTime.Now,
                UsuarioId = usuarioId
            };
            await _dataContext.Mensajes.AddAsync(respuestaIA);
            await _dataContext.SaveChangesAsync();
            Mensajes.Add(respuestaIA);
        }

        [RelayCommand]
        public void AbrirMenu() => Shell.Current.FlyoutIsPresented = true;

        [RelayCommand]
        public async Task IrAPerfil() => await Shell.Current.GoToAsync("Perfil");
    }
}