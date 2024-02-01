using System;
using System.Collections.Generic;
using System.Text;
using TestAptitudes.Model;

namespace TestAptitudes.Services
{
    public class UsuarioService
    {
        public List<UsuarioModel> GetUsuarios()
        {
            return GetUsuariosMockup();
        }

        private List<UsuarioModel> GetUsuariosMockup()
        {
            return new List<UsuarioModel>
            {
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
    }
}
