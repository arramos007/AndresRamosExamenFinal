using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndresRamosExamenFinal.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AndresRamosExamenFinal.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailProductoPage : ContentPage
    {
        public DetailProductoPage(DetailProductoPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {

        }
    }
}