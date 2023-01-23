using Newtonsoft.Json;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Exceptions;
using SurPrise.Models;
using SurPrise.Services;
using SurPrise.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
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

            // Actualisation de l'affichage de la liste des prises
            _viewModel.RefreshPlugList.Execute(this);
        }

        // Ajout d'une prise (bouton en haut à droite)
        public async void AddPlug(object sender, EventArgs e)
        {
            // Préparation du Bluetooth LE
            var ble = CrossBluetoothLE.Current;
            var adapter = CrossBluetoothLE.Current.Adapter;
            var scanFilterOptions = new ScanFilterOptions();
            //scanFilterOptions.DeviceAddresses = new [] { "00:06:66:D0:B6:7B" };
            adapter.ScanMode = Plugin.BLE.Abstractions.Contracts.ScanMode.LowPower;

            var state = ble.State;

            // Vérification que le Bluetooth est activé
            // !!! Depuis les dernières versions d'Android, la localisation est nécessaire en plus du Bluetooth
            if (state != Plugin.BLE.Abstractions.Contracts.BluetoothState.On)
            {
                await DisplayAlert("Bluetooth désactivé", "Activez le Bluetooth avant de réessayer", "OK");
            }
            else
            {
                // Scan Bluetooth
                _ = this.DisplayToastAsync("Recherche d'appareils en cours...", 10000);
                List<Plugin.BLE.Abstractions.Contracts.IDevice> deviceList = new List<Plugin.BLE.Abstractions.Contracts.IDevice>();
                adapter.DeviceDiscovered += (s, a) => { if (a.Device.Name != null) deviceList.Add(a.Device); };
                await adapter.StartScanningForDevicesAsync(scanFilterOptions);
                List<string> deviceNames = new List<string>();
                foreach (var device in deviceList)
                    deviceNames.Add(device.Name);

                // Affichage des appareils trouvés
                var selectedDevice = await DisplayActionSheet("Appareils disponibles (" + deviceNames.Count + ")", "Annuler", null, deviceNames.ToArray());

                if (selectedDevice != null && selectedDevice != "Annuler")
                {
                    var device = deviceList[deviceNames.FindIndex((element) => element == selectedDevice)];

                    // Choix du nom de la prise dans l'appli
                    string name = await DisplayPromptAsync("Nouvelle prise", "Nom de la prise :", "Continuer", "Annuler", device.Name);
                    bool ok;
                    List<(string name, string desc, bool on, Guid id)> plugs = Settings.PlugListContent;
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
                            name = await DisplayPromptAsync("Nouvelle prise", "Ce nom existe déjà.\nChoisissez-en un autre :", "Continuer", "Annuler");
                    } while (!ok);

                    if (name != null && name != "")
                    {
                        // Choix de la derscription de la prise
                        string description = await DisplayPromptAsync("Nouvelle prise", "Description de la prise (optionnel) :", "Continuer", "Annuler");
                        if (description != null)
                        {
                            // Ajout de la prise au module Settings
                            plugs.Add((name: name, desc: description, on: false, id: device.Id));
                            Settings.PlugListContent = plugs;

                            _viewModel.RefreshPlugList.Execute(this);

                            // Connexion à la prise
                            try
                            {
                                await adapter.ConnectToDeviceAsync(device);
                                //var service = await device.GetServiceAsync(Guid.Parse("00000000-0000-1000-8000-00805F9B34FB"));
                                await DisplayAlert("Connexion réussie", "L'appareil " + device.Name + " est bien connecté.", "OK");
                            }
                                catch (DeviceConnectionException ex)
                            {
                                await DisplayAlert("Connexion impossible", "L'appareil " + device.Name + " est peut-être déconnecté ou trop loin.", "OK");
                                Console.WriteLine(ex);
                            }
                        }
                    }
                }
            }
        }

        // Ouverture de la page spécifique à une prise lorsque la CustomCell associée est cliquée
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