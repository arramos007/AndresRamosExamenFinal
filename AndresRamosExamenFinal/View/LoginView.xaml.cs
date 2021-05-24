using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndresRamosExamenFinal.Service;
using AndresRamosExamenFinal.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AndresRamosExamenFinal.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }

        private void TapBackArrow_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

    }
}