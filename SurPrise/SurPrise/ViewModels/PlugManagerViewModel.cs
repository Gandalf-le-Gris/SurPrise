using SurPrise.Models;
using SurPrise.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SurPrise.ViewModels
{
    public class PlugManagerViewModel : BaseViewModel
    {
        public ObservableCollection<CustomCell> Cells { get; set; }
        public PlugManagerViewModel()
        {
            Title = "Mes prises";
            Cells = new ObservableCollection<CustomCell>();


            // Actualisation graphique de la liste de prises en se basant sur le viewmodel pour stocker les données (MVVM)
            RefreshPlugList = new Command(() =>
            {
                Cells.Clear();
                foreach ((string name, string desc, _, _) in Settings.PlugListContent)
                {
                    CustomCell cell = new CustomCell
                    {
                        Title = name,
                        Detail = desc
                    };
                    Cells.Add(cell);
                }
            });
        }

        public ICommand RefreshPlugList;
    }
}