using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using AndresRamosExamenFinal.Model;
using Xamarin.Forms;

namespace AndresRamosExamenFinal.ViewModel
{
    public class AddProductoPageViewModel : INotifyPropertyChanged
    {
        bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBusy)));
            }
        }
        public AddProductoPageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            Save = new Command(HandleSave);
            Cancel = new Command(HandleCancel);
        }

        private INavigation _navigation;
        private string codigo_principal_producto;

        public string Codigo_principal_producto
        {
            get { return codigo_principal_producto; }
            set
            {
                codigo_principal_producto = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCompleted)));
            }
        }

        private bool isCompleted;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsCompleted
        {
            get { return isCompleted; }
            set
            {
                isCompleted = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCompleted)));
            }
        }

        public Command Save { get; set; }
        public async void HandleSave()
        {
            IsBusy = true;
            await App.Database.SaveProductoModelAsync(new ProductoModel 
            { codigo_principal_producto = codigo_principal_producto, isCompleted = IsCompleted == true ? 1 : 0 });
            IsBusy = false;
            await _navigation.PopModalAsync();
        }

        public Command Cancel { get; set; }
        public async void HandleCancel()
        {
            await _navigation.PopModalAsync();
        }
    }
}
