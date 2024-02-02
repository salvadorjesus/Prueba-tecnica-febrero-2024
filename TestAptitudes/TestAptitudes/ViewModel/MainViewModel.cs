using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TestAptitudes.Model;
using TestAptitudes.Services;
using Xamarin.Forms;

namespace TestAptitudes.ViewModel
{
    public class MainViewModel
    {
        public ICommand AddUserCmd => new Command(AddUser);
        public ICommand AddUserListCmd => new Command(LoadUsers);
        public ICommand SwitchSelectedCmd => new Command(SwitchUserSelectedStatus);

        public ObservableCollection<UsuarioViewModel> Usuarios { get; set; }

        private UsuarioService UsuarioService;

        public MainViewModel()
        {
            UsuarioService = new UsuarioService();

            Usuarios = new ObservableCollection<UsuarioViewModel>();
            LoadUsers();
        }

        private async void LoadUsers()
        {
            var usuarios = await UsuarioService.GetUsuarios();
            foreach (var usuario in usuarios)
                Usuarios.Add( new UsuarioViewModel(usuario));
        }

        private void AddUser(object obj)
        {
            Usuarios.Add(new UsuarioViewModel() { Nombre = "Nuevo", Apellido = "Usuario", Telefono = "999 88 87 77" });
        }

        private void SwitchUserSelectedStatus(object selectedUsuario)
        {
            var usuario = selectedUsuario as UsuarioViewModel;
            Console.WriteLine($"Clic en el usuario: {usuario.Nombre} {usuario.Apellido}");
            usuario.Seleccionado = !usuario.Seleccionado;
        }
    }
}

