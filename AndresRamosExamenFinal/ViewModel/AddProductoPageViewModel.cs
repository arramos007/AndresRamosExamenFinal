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
        private string codigo_auxiliar_producto;
        private string nombre;
        private float valor_unitario;
        private int tipo_productos_id;
        private int users_id;
        private bool isCompleted;
        public event PropertyChangedEventHandler PropertyChanged;
        public string CodigoPrincipalProducto
        {
            get { return codigo_principal_producto; }
            set
            {
                codigo_principal_producto = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CodigoPrincipalProducto)));
            }
        }

        public string CodigoAuxiliarProducto
        {
            get { return codigo_auxiliar_producto; }
            set
            {
                codigo_auxiliar_producto = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CodigoAuxiliarProducto)));
            }
        }
        public string Nombre
        {
            get { return nombre; }
            set
            {
                nombre = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Nombre)));
            }
        }
        public float ValorUnitario
        {
            get { return valor_unitario; }
            set
            {
                valor_unitario = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValorUnitario)));
            }
        }

        public int TipoProductosId
        {
            get { return tipo_productos_id; }
            set
            {
                tipo_productos_id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TipoProductosId)));
            }
        }

        public int UsersId
        {
            get { return users_id; }
            set
            {
                users_id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UsersId)));
            }
        }
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
            { 
                codigo_principal_producto = CodigoPrincipalProducto,
                codigo_auxiliar_producto = CodigoAuxiliarProducto,
                nombre = Nombre,
                valor_unitario = ValorUnitario,
                tipo_productos_id = TipoProductosId,
                users_id = UsersId,
                isCompleted = IsCompleted == true ? 1 : 0 
            });
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
