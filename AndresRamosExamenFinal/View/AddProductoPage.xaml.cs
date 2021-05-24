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
    public partial class AddProductoPage : ContentPage
    {
        public AddProductoPage()
        {
            InitializeComponent();
            BindingContext = new AddProductoPageViewModel(Navigation);
        }
    }
}