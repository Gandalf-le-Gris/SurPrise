using SurPrise.ViewModels;
using SurPrise.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace SurPrise
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(PlugDetailPage), typeof(PlugDetailPage));
            Routing.RegisterRoute(nameof(PlugManagerPage), typeof(PlugManagerPage));
        }

    }
}
