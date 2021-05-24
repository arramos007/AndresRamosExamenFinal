using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AndresRamosExamenFinal.Data;
using AndresRamosExamenFinal.Model;
using AndresRamosExamenFinal.Service;
using AndresRamosExamenFinal.View;
using AndresRamosExamenFinal.ViewModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace AndresRamosExamenFinal
{
    public partial class App : Application
    {
        static ProductoDatabase database;
        public static ProductoDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new ProductoDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NProducto.db3"));
                }
                return database;
            }
        }
        public App()
        {
            InitializeComponent();
            var access_token = Preferences.Get("access_token", string.Empty);
            if (string.IsNullOrEmpty(access_token))
                MainPage = new NavigationPage(new SignupView());
            else
                MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            CrossConnectivity.Current.ConnectivityChanged += HandleConnectivityChanged;
        }

        private void HandleConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            Type currentPage = this.MainPage.GetType();
            if (e.IsConnected && currentPage != typeof(MainPage))
                this.MainPage = new MainPage();
            else if (!e.IsConnected && currentPage != typeof(MainPage))
                this.MainPage = new MainPage();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
