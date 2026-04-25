using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PettoV1.Views;
using SharedResources.Data;
using SharedResources.Models;
using System.Collections.ObjectModel;

namespace PettoV1.ViewModels
{
    public partial class TareasViewModel : ObservableObject
    {
        private readonly DataContext _dataContext;

        [ObservableProperty]
        private ObservableCollection<TareaModel> _tareasProximas = new();

        [ObservableProperty]
        private ObservableCollection<CategoriaModel> _categorias = new();

        [ObservableProperty]
        private string _fechaHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

        public TareasViewModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task InicializarAsync()
        {
            await CargarTareasAsync();
            await CargarCategoriasAsync();
        }

        private async Task CargarTareasAsync()
        {
            TareasProximas.Clear();
            var tareas = await _dataContext.Tareas
                .AsNoTracking()
                .Where(t => !t.Completada && t.FechaLimite >= DateTime.Today)
                .OrderBy(t => t.FechaLimite)
                .Take(10)
                .ToListAsync();
            foreach (var t in tareas) TareasProximas.Add(t);
        }

        private async Task CargarCategoriasAsync()
        {
            Categorias.Clear();
            var cats = await _dataContext.Categorias
                .AsNoTracking()
                .Include(c => c.Tareas)
                .ToListAsync();
            foreach (var c in cats) Categorias.Add(c);
        }

        [RelayCommand]
        public async Task VerCategoria(CategoriaModel categoria)
        {
            await Shell.Current.GoToAsync(
                nameof(Categoria),
                new Dictionary<string, object> { ["Categoria"] = categoria });
        }

        [RelayCommand]
        public async Task AbrirMenu()
        {
            Shell.Current.FlyoutIsPresented = true;
        }

        [RelayCommand]
        public async Task IrAPerfil()
        {
            await Shell.Current.GoToAsync(nameof(Perfil));
        }
    }
}