using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TestAptitudes.Model;
using Xamarin.Forms;

namespace TestAptitudes.ViewModel
{
    public class MainViewModel
    {
        public List<UsuarioModel> Usuarios { get; set; }

        public ICommand AddUserCmd => new Command(AddUsuarioCmdExecute);

        public MainViewModel()
        {
            Usuarios = new List<UsuarioModel>();
            AddUsuarioALista();
        }

        private List<UsuarioModel> getUsuarios()
        {
            return new List<UsuarioModel> {
                new UsuarioModel() { Nombre = "Nuria", Apellido = "García", Telefono = "999 88 87 77" , Seleccionado=false},
                new UsuarioModel() { Nombre = "Juan", Apellido = "López", Telefono = "999 66 44 33", Seleccionado=true },
                new UsuarioModel() { Nombre = "María", Apellido = "Fernandez", Telefono = "999 44 33 22", Seleccionado=false },
                new UsuarioModel() { Nombre = "Bob", Apellido = "Williams", Telefono = "999 44 87 44", Seleccionado=true },
                new UsuarioModel() { Nombre = "Gorka", Apellido = "Izaguirre", Telefono = "999 11 22 33", Seleccionado=true },
                new UsuarioModel() { Nombre = "Joan", Apellido = "Keller", Telefono = "999 77 77 77", Seleccionado=false },
                new UsuarioModel() { Nombre = "Alberto", Apellido = "Jimenez", Telefono = "999 33 22 11", Seleccionado=false },
                new UsuarioModel() { Nombre = "Pedro", Apellido = "Galarza", Telefono = "999 00 11 22", Seleccionado=true },
                new UsuarioModel() { Nombre = "Jon", Apellido = "Barrios", Telefono = "999 55 77 22", Seleccionado=false }
            };
        }

        private void AddUsuarioALista()
        {
            foreach (var usuario in getUsuarios())
                Usuarios.Add(usuario);
        }

        private void AddUsuarioCmdExecute(object obj)
        {
            Usuarios.Add(new UsuarioModel() { Nombre = "Nuevo", Apellido = "Usuario", Telefono = "999 88 87 77" });
        }
    }
}

