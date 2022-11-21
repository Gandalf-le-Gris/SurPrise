using SurPrise.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace SurPrise.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}