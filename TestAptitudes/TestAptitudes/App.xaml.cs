using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestAptitudes
{
    public partial class App : Application
    {
        public App ()
        {
            InitializeComponent();

            MainPage = new View.MainPage();
        }

        protected override void OnStart ()
        {
        }

        protected override void OnSleep ()
        {
        }

        protected override void OnResume ()
        {
        }
    }
}

