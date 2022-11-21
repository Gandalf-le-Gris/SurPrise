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

            RefreshPlugList = new Command(() =>
            {
                Cells.Clear();
                foreach ((string name, string desc, _) in Settings.PlugListContent)
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