using Microcharts;
using SkiaSharp;
using SurPrise.Models;
using SurPrise.Services;
using SurPrise.ViewModels;
using SurPrise.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SurPrise.Views
{
    [QueryProperty("Index", "index")]

    public partial class PlugDetailPage : ContentPage
    {
        private ChartEntry[] entriesDaily = new ChartEntry[24];
        private ChartEntry[] entriesMonthly = new ChartEntry[30];

        private string _index = "0";

        public string Index
        {
            set
            {
                _index = value;
                var plug = Settings.PlugListContent[int.Parse(value)];
                Name.Text = plug.name;
                Description.Text = plug.desc;
            }
            get
            {
                return _index;
            }
        }

        public PlugDetailPage()
        {
            InitializeComponent();

            Random rd = new Random();

            for (int i = 0; i < entriesDaily.Length; i++) {
                int value = rd.Next(0, 255);
                //Color color = Color.FromRgb(.4 + Math.Floor(value / 2.0) / 255, .9 - Math.Floor(value / 2.0) / 255, .5);
                Color color = value < 128 ? Color.Green : Color.Red;
                int time = DateTime.Now.AddDays(-1).AddHours(i + 1).Hour;
                entriesDaily[i] = new ChartEntry(value)
                {
                    Color = SKColor.Parse(color.ToHex()),
                    ValueLabel = "1",
                    Label = (time % 3 == 0) ? time.ToString() + "h" : "",
                    ValueLabelColor = SKColor.Parse(((Color)Application.Current.Resources["LightGreen"]).ToHex())
            };
            }

            LineChart chart = new LineChart { Entries = entriesDaily };
            chart.BackgroundColor = SKColors.Transparent;
            chart.LabelColor = SKColor.Parse(((Color)Application.Current.Resources["DarkGreen"]).ToHex());
            chart.LabelTextSize = (float)Device.GetNamedSize(NamedSize.Large, typeof(Label));
            dailyConsumption.Chart = chart;

            for (int i = 0; i < entriesMonthly.Length; i++)
            {
                int value = rd.Next(0, 255);
                //Color color = Color.FromRgb(.4 + Math.Floor(value / 2.0) / 255, .9 - Math.Floor(value / 2.0) / 255, .5);
                Color color = value < 128 ? Color.Green : Color.Red;
                entriesMonthly[i] = new ChartEntry(value)
                {
                    Color = SKColor.Parse(color.ToHex()),
                    ValueLabel = "1",
                    Label = (i % 3 == 0) ? DateTime.Now.AddDays(-(entriesMonthly.Length - i)).ToString().Substring(0, 5) : "",
                    ValueLabelColor = SKColor.Parse(((Color)Application.Current.Resources["LightGreen"]).ToHex())
                };
            }

            LineChart chart2 = new LineChart { Entries = entriesMonthly };
            chart2.BackgroundColor = SKColors.Transparent;
            chart2.LabelColor = SKColor.Parse(((Color)Application.Current.Resources["DarkGreen"]).ToHex());
            chart2.LabelTextSize = (float)Device.GetNamedSize(NamedSize.Large, typeof(Label));
            monthlyConsumption.Chart = chart2;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            dailyConsumption.Chart.AnimateAsync(true);
            monthlyConsumption.Chart.AnimateAsync(true);
        }

        public async void ToggleOnOff(object sender, EventArgs e)
        {
            ImageButton s = (ImageButton)sender;
            await s.ScaleTo(0.9, 75);
            await s.ScaleTo(1, 75);
        }

        public async void EditPlug(object sender, EventArgs e)
        {
            string name = await DisplayPromptAsync("Modifier la prise", "Nouveau nom :");
            bool ok;
            int i;
            List<(string name, string desc, bool on)> plugs = Settings.PlugListContent;
            do
            {
                ok = true;
                i = 0;
                foreach (var p in plugs)
                {
                    if (p.name == name && i != int.Parse(Index))
                    {
                        ok = false;
                        break;
                    }
                    i++;
                }
                if (!ok)
                    name = await DisplayPromptAsync("Nouvelle prise", "Ce nom existe déjà.\nChoisissez-en un autre :");
            } while (!ok);
            if (name == null)
                return;
            if (name == "")
                name = Name.Text;

            string description = await DisplayPromptAsync("Nouvelle prise (optionnel)", "Description de la prise :");

            if (description == null)
                description = Description.Text;

            var plug = plugs[int.Parse(Index)];
            plug.name = name;
            plug.desc = description;
            plugs[int.Parse(Index)] = plug;
            Settings.PlugListContent = plugs;
            Name.Text = name;
            Description.Text = description;
        }

        public async void DeletePlug(object sender, EventArgs e)
        {
            bool ans = await DisplayAlert("Attention !", "Êtes-vous sûr de vouloir supprimer cette prise ?", "Oui", "Non");
            if (ans)
            {
                var list = Settings.PlugListContent;
                list.RemoveAt(int.Parse(Index));
                Settings.PlugListContent = list;

                await Shell.Current.GoToAsync("..", true);
            }
        }
    }
}