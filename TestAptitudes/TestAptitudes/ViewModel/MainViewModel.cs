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
        public List<UsuarioModel> Usuarios { get; set; }

        public ICommand AddUserCmd => new Command(AddUsuarioCmdExecute);

        private UsuarioService UsuarioService;

        public MainViewModel()
        {
            UsuarioService = new UsuarioService();

            Usuarios = new List<UsuarioModel>();
            AddUsuarioALista();
        }

        private async void AddUsuarioALista()
        {
            var usuarios = await UsuarioService.GetUsuarios();
            foreach (var usuario in usuarios)
                Usuarios.Add(usuario);
        }

        private void AddUsuarioCmdExecute(object obj)
        {
            Usuarios.Add(new UsuarioModel() { Nombre = "Nuevo", Apellido = "Usuario", Telefono = "999 88 87 77" });
        }
    }
}

