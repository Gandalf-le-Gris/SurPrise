using SurPrise.Models;
using SurPrise.Services;
using SurPrise.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SurPrise.Views
{
    public partial class PlugManagerPage : ContentPage
    {
        PlugManagerViewModel _viewModel;

        public PlugManagerPage()
        {
            InitializeComponent();

            _viewModel = (PlugManagerViewModel)this.BindingContext;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.RefreshPlugList.Execute(this);
        }

        public async void AddPlug(object sender, EventArgs e)
        {
            string name = await DisplayPromptAsync("Nouvelle prise", "Nom de la prise :");
            bool ok;
            List<(string name, string desc, bool on)> plugs = Settings.PlugListContent;
            do
            {
                ok = true;
                foreach (var plug in plugs)
                {
                    if (plug.name == name)
                    {
                        ok = false;
                        break;
                    }
                }
                if (!ok)
                    name = await DisplayPromptAsync("Nouvelle prise", "Ce nom existe déjà.\nChoisissez-en un autre :");
            } while (!ok);

            if (name != null && name != "")
            {
                string description = await DisplayPromptAsync("Nouvelle prise", "Description de la prise (optionnel) :");
                if (description != null)
                {
                    plugs.Add((name: name, desc: description, on: false));
                    Settings.PlugListContent = plugs;

                    _viewModel.RefreshPlugList.Execute(this);
                }
            }
        }

        public async void OpenPlugDetail(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                int index = 0;
                foreach (CustomCell c in ((ListView)sender).ItemsSource)
                {
                    if (c.Title == ((CustomCell)e.SelectedItem).Title)
                        break;
                    index++;
                }
                await Shell.Current.GoToAsync($"PlugDetailPage?index=" + index, true);
                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}