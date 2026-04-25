using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using SharedResources.Data;
using SharedResources.Models;
using System.Collections.ObjectModel;

namespace PettoV1.ViewModels
{
    public partial class TareasViewModel : ObservableObject
    {
        private readonly DataContext _dataContext;

        [ObservableProperty] private ObservableCollection<TareaModel> _tareasProximas = new();
        [ObservableProperty] private ObservableCollection<CategoriaModel> _categorias = new();
        [ObservableProperty] private string _fechaHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

        public TareasViewModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task InicializarAsync()
        {
            await CargarCategoriasAsync();
            await CargarTareasAsync();
        }

        private async Task CargarTareasAsync()
        {
            int usuarioId = Preferences.Get("UsuarioId", 0);

            TareasProximas.Clear();
            var tareas = await _dataContext.Tareas
                .AsNoTracking()
                .Include(t => t.Categoria)
                .Where(t => !t.Completada &&
                            t.FechaLimite >= DateTime.Today &&
                            t.Categoria.UsuarioId == usuarioId)
                .OrderBy(t => t.FechaLimite)
                .Take(10)
                .ToListAsync();
            foreach (var t in tareas) TareasProximas.Add(t);
        }

        private async Task CargarCategoriasAsync()
        {
            int usuarioId = Preferences.Get("UsuarioId", 0);

            Categorias.Clear();
            var cats = await _dataContext.Categorias
                .AsNoTracking()
                .Where(c => c.UsuarioId == usuarioId)
                .ToListAsync();
            foreach (var c in cats) Categorias.Add(c);
        }

        [RelayCommand]
        public async Task AgregarCategoria()
        {
            int usuarioId = Preferences.Get("UsuarioId", 0);

            string nombre = await Shell.Current.DisplayPromptAsync(
                "Nueva categoría",
                "Escribe el nombre de la categoría:",
                accept: "Guardar",
                cancel: "Cancelar",
                placeholder: "Ej. Salud, Ejercicio...",
                maxLength: 50);

            if (string.IsNullOrWhiteSpace(nombre)) return;

            bool existe = await _dataContext.Categorias
                .AnyAsync(c => c.Nombre.ToLower() == nombre.ToLower() &&
                               c.UsuarioId == usuarioId);

            if (existe)
            {
                await Shell.Current.DisplayAlert("Aviso", "Ya existe esa categoría.", "OK");
                return;
            }

            var nueva = new CategoriaModel
            {
                Nombre = nombre,
                UsuarioId = usuarioId
            };

            await _dataContext.Categorias.AddAsync(nueva);
            await _dataContext.SaveChangesAsync();
            await CargarCategoriasAsync();
        }

        [RelayCommand]
        public async Task VerCategoria(CategoriaModel categoria)
        {
            await Shell.Current.GoToAsync(
                "Categoria",
                new Dictionary<string, object> { ["Categoria"] = categoria });
        }

        [RelayCommand]
        public async Task EliminarCategoria(CategoriaModel categoria)
        {
            string resp = await Shell.Current.DisplayActionSheet(
                $"¿Eliminar '{categoria.Nombre}'?", "Cancelar", "Eliminar");
            if (resp != "Eliminar") return;

            var entidad = await _dataContext.Categorias.FindAsync(categoria.Id);
            if (entidad is not null)
            {
                _dataContext.Categorias.Remove(entidad);
                await _dataContext.SaveChangesAsync();
                await CargarCategoriasAsync();
            }
        }

        [RelayCommand]
        public void AbrirMenu() => Shell.Current.FlyoutIsPresented = true;

        [RelayCommand]
        public async Task IrAPerfil() => await Shell.Current.GoToAsync("Perfil");
    }
}