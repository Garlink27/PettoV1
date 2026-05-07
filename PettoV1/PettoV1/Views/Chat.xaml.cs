using PettoV1.ViewModels;

namespace PettoV1.Views
{
    public partial class Chat : ContentPage
    {
        private readonly ChatViewModel _vm;

        public Chat(ChatViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _vm.InicializarAsync();
        }
    }
}