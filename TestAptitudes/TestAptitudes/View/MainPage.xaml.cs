using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAptitudes.ViewModel;
using Xamarin.Forms;

namespace TestAptitudes.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            this.BindingContext = new MainViewModel();

            MessagingCenter.Subscribe<MainViewModel, UsuarioViewModel>(this, "Single new user added", (sender, nuevoUsuario) =>
            {
                UserCollection.ScrollTo(nuevoUsuario, position: ScrollToPosition.MakeVisible, animate: true);
            });

        }
    }         
}

