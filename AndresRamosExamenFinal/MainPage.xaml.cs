using AndresRamosExamenFinal.Model;
using AndresRamosExamenFinal.ViewModel;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace AndresRamosExamenFinal
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel(Navigation);
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as MainPageViewModel).RefreshTaskList();
            CrossConnectivity.Current.ConnectivityChanged += UpdateNetworkInfo;
        }

        protected override void OnDisappearing()
        {
            CrossConnectivity.Current.ConnectivityChanged -= UpdateNetworkInfo;
        }

        private async void UpdateNetworkInfo(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            await (BindingContext as MainPageViewModel).RefreshTaskList();
        }

        private async void ProductoDisplayList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await (BindingContext as MainPageViewModel).HandleNoteSelected((ProductoModel)e.SelectedItem);
        }
    }
}
