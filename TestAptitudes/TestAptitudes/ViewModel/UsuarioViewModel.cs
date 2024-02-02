using System.ComponentModel;
using TestAptitudes.Model;

namespace TestAptitudes.ViewModel
{
    /**Wrapper class for the UsuarioModel class that implements change notifications.
     * That allows the view to react to changes. Discussion on why this approach has
     * been chosen among others can be found in the Readme of the project.*/
    public class UsuarioViewModel : INotifyPropertyChanged
    {
        private UsuarioModel _usuario;


        public UsuarioViewModel()
        {
            _usuario = new UsuarioModel();
        }
        public UsuarioViewModel(UsuarioModel usuario)
        {
            _usuario = usuario;
        }

        public string Nombre
        {
            get { return _usuario.Nombre; }
            set
            {
                if (_usuario.Nombre != value)
                {
                    _usuario.Nombre = value;
                    OnPropertyChanged(nameof(Nombre));
                }
            }
        }

        public string Apellido
        {
            get { return _usuario.Apellido; }
            set
            {
                if (_usuario.Apellido != value)
                {
                    _usuario.Apellido = value;
                    OnPropertyChanged(nameof(Apellido));
                }
            }
        }

        public string Telefono
        {
            get { return _usuario.Telefono; }
            set
            {
                if (_usuario.Telefono != value)
                {
                    _usuario.Telefono = value;
                    OnPropertyChanged(nameof(Telefono));
                }
            }
        }

        public bool Seleccionado
        {
            get { return _usuario.Seleccionado; }
            set
            {
                if (_usuario.Seleccionado != value)
                {
                    _usuario.Seleccionado = value;
                    OnPropertyChanged(nameof(Seleccionado));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
