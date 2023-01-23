// Classe permettant de générer des cellules personnalisées avec de bons bindings pour la liste de prises dans le Plug Manager

using SurPrise.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SurPrise.Models
{
    public class CustomCell : ViewCell
    {
        // Attributs de classe
        readonly Label title, detail;
        readonly BoxView statusColor;

        public ImageButton onOff;

        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create("Title", typeof(string), typeof(CustomCell), "");
        public static readonly BindableProperty DetailProperty =
            BindableProperty.Create("Detail", typeof(string), typeof(CustomCell), "");

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public string Detail
        {
            get { return (string)GetValue(DetailProperty); }
            set { SetValue(DetailProperty, value); }
        }

        // Modification dynamique des informations depuis les bindings
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                title.Text = Title;
                detail.Text = Detail;
            }
        }

        // Construction de l'élément cellule
        public CustomCell()
        {

            //instantiate each of our views
            StackLayout cellWrapper = new StackLayout();
            Grid mainLayout = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(30, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star) }
                },
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }
                }
            };
            StackLayout textLayout = new StackLayout();
            Frame frame = new Frame();
            this.title = new Label();
            this.detail = new Label();
            this.onOff = new ImageButton();
            this.statusColor = new BoxView();

            //set bindings
            title.SetBinding(Label.TextProperty, "title");
            detail.SetBinding(Label.TextProperty, "detail");

            //Set properties for desired design
            //cellWrapper.VerticalOptions = LayoutOptions.CenterAndExpand;
            cellWrapper.HeightRequest = 90;
            frame.Padding = new Thickness(10, 5);
            frame.Margin = new Thickness(15, 10);
            frame.BackgroundColor = (Color)Application.Current.Resources["LightGreen"];
            frame.BorderColor = (Color)Application.Current.Resources["Green"];
            frame.HasShadow = false;
            frame.CornerRadius = 10;
            frame.VerticalOptions = LayoutOptions.FillAndExpand;
            frame.HorizontalOptions = LayoutOptions.FillAndExpand;
            mainLayout.VerticalOptions = LayoutOptions.CenterAndExpand;
            mainLayout.HorizontalOptions = LayoutOptions.CenterAndExpand;
            textLayout.Orientation = StackOrientation.Vertical;
            textLayout.Spacing = 5;
            textLayout.VerticalOptions = LayoutOptions.CenterAndExpand;
            textLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
            title.TextColor = (Color)Application.Current.Resources["DarkGreen"];
            title.FontAttributes = FontAttributes.Bold;
            title.LineBreakMode = LineBreakMode.TailTruncation;
            title.HorizontalOptions = LayoutOptions.FillAndExpand;
            title.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            detail.LineBreakMode = LineBreakMode.TailTruncation;
            detail.TextColor = (Color)Application.Current.Resources["Green"];
            detail.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
            statusColor.Color = (Color)Application.Current.Resources["Off"];
            statusColor.HorizontalOptions = LayoutOptions.FillAndExpand;
            statusColor.VerticalOptions = LayoutOptions.FillAndExpand;
            onOff.BackgroundColor = Color.Transparent;
            onOff.Source = "power.png";
            onOff.Aspect = Aspect.AspectFill;
            onOff.HeightRequest = 40;

            TapGestureRecognizer tapped = new TapGestureRecognizer();
            

            tapped.Tapped += (s, e) => ClickAnimation(s, e);
            frame.GestureRecognizers.Add(tapped);
            onOff.Clicked += (s, e) => { ToggleOnOff(s, e); };
            

            //add views to the view hierarchy
            mainLayout.Children.Add(statusColor, 0, 0);
            mainLayout.Children.Add(textLayout, 1, 0);
            mainLayout.Children.Add(onOff, 2, 0);
            textLayout.Children.Add(title);
            textLayout.Children.Add(detail);
            frame.Content = mainLayout;
            cellWrapper.Children.Add(frame);
            View = cellWrapper;
        }

        public CustomCell Clone()
        {
            return new CustomCell
            {
                Title = this.Title,
                Detail = this.Detail,
            };
        }

        // Animation de clic (enfoncement)
        public async void ClickAnimation(object sender, EventArgs e)
        {
            View s = (View)sender;
            await s.ScaleTo(0.9, 75);
            await s.ScaleTo(1, 75);
            var parent = this.Parent as ListView;
            parent.SelectedItem = this;
        }

        // Bouton on/off
        public async void ToggleOnOff(object sender, EventArgs e)
        {
            ImageButton s = (ImageButton)sender;
            await s.ScaleTo(0.9, 75);
            await s.ScaleTo(1, 75);
            statusColor.Color = statusColor.Color == (Color)Application.Current.Resources["On"] ? (Color)Application.Current.Resources["Off"] : (Color)Application.Current.Resources["On"];
        }
    }
}
