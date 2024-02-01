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

        public ObservableCollection<UsuarioModel> Usuarios { get; set; }
        private UsuarioService UsuarioService;

        public MainViewModel()
        {
            UsuarioService = new UsuarioService();

            Usuarios = new ObservableCollection<UsuarioModel>();
            LoadUsers();
        }

        private async void LoadUsers()
        {
            var usuarios = await UsuarioService.GetUsuarios();
            foreach (var usuario in usuarios)
                Usuarios.Add(usuario);
        }

        private void AddUser(object obj)
        {
            Usuarios.Add(new UsuarioModel() { Nombre = "Nuevo", Apellido = "Usuario", Telefono = "999 88 87 77" });
        }
    }
}

