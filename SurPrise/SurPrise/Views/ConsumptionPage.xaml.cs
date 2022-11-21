using Microcharts;
using SkiaSharp;
using SurPrise.Models;
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
    public partial class ConsumptionPage : ContentPage
    {
        private ChartEntry[] entriesDaily = new ChartEntry[24];
        private ChartEntry[] entriesMonthly = new ChartEntry[30];

        public ConsumptionPage()
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
    }
}