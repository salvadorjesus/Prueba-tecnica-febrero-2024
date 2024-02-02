using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TestAptitudes.Model;
using TestAptitudes.Services;
using Xamarin.Forms;

namespace TestAptitudes.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ICommand AddUserCmd => new Command(AddSingleUser);
        public ICommand AddUserListCmd => new Command(LoadUsers);
        public ICommand SwitchSelectedCmd => new Command(SwitchUserSelectedStatus);

        public ObservableCollection<UsuarioViewModel> Usuarios { get; set; }

        private bool _isLoading;
        /**Observable property for the view.
         * Also avoid concurrent loading.*/
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        private UsuarioService UsuarioService;

        public MainViewModel()
        {
            UsuarioService = new UsuarioService();
            Usuarios = new ObservableCollection<UsuarioViewModel>();
            AddUserListCmd.Execute(null);
        }

        /* Commands excecute functions */
        private async void LoadUsers()
        {
            //Ignore new calls if LoadUsers is already running.
            //This can happen due to the infinite scroll mechanism with RemainingItemsThreshold.
            if (IsLoading)
                return;
            IsLoading = true;

            var usuarios = await UsuarioService.GetUsuarios();
            foreach (var usuario in usuarios)
                Usuarios.Add(new UsuarioViewModel(usuario));

            IsLoading = false;
        }

        private void AddSingleUser(object obj)
        {
            var newUser = new UsuarioViewModel() { Nombre = "Nuevo", Apellido = "Usuario", Telefono = "999 88 87 77" };
            Usuarios.Add(newUser);
            MessagingCenter.Send(this, "Single new user added", newUser);
        }

        private void SwitchUserSelectedStatus(object selectedUsuario)
        {
            var usuario = selectedUsuario as UsuarioViewModel;
            usuario.Seleccionado = !usuario.Seleccionado;
        }

        /*  INotifyPropertyChanged implementation */
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
