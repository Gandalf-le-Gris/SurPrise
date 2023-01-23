using SurPrise.Services;
using SurPrise.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SurPrise
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            // Possibilité de connexion par défaut à toutes les prises connues pour obtenir les informations accumulées ici
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
