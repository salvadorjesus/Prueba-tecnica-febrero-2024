using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestAptitudes.Model;

namespace TestAptitudes.Services
{
    public class UsuarioService
    {
        /** Use to emulate a delay when getting users using the using the mockup function.
         * This kind of user data usually come from the internet, a data base or from disc,
         * so we are avoiding a future technical debt by writting asynchronous code.*/
        public int MockupDelay = 0;
        public async Task<List<UsuarioModel>> GetUsuarios()
        {
            //A factory method could be implemented here if needed.
            return await GetUsuariosMockup();
        }

        private async Task<List<UsuarioModel>> GetUsuariosMockup()
        {
            await Task.Delay(MockupDelay);

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
