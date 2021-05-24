using System;
using System.ComponentModel;
using System.Windows.Input;
using AndresRamosExamenFinal.Model;
using Xamarin.Forms;

namespace AndresRamosExamenFinal.ViewModel
{
    public class DetailProductoPageViewModel : INotifyPropertyChanged
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
        public DetailProductoPageViewModel()
        {
            ExitCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopAsync());
            UpdateCommand = new Command(HandleSave);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public async void HandleSave()
        {
            await App.Database.SaveProductoModelAsync(new ProductoModel 
            { 
                id = Id, 
                codigo_principal_producto = CodigoPrincipalProducto,
                codigo_auxiliar_producto = CodigoAuxiliarProducto,
                nombre = Nombre,
                valor_unitario = ValorUnitario,
                tipo_productos_id = TipoProductosId,
                users_id = UsersId,
                isCompleted = IsCompleted == true ? 1 : 0 
            });
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        int id;
        string codigo_principal_producto;
        string codigo_auxiliar_producto;
        string nombre;
        float valor_unitario;
        int tipo_productos_id;
        int users_id;
        bool isCompleted;
        public string CodigoPrincipalProducto
        {
            get => codigo_principal_producto;
            set
            {
                codigo_principal_producto = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CodigoPrincipalProducto)));
            }
        }
        public string CodigoAuxiliarProducto
        {
            get => codigo_auxiliar_producto;
            set
            {
                codigo_auxiliar_producto = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CodigoAuxiliarProducto)));
            }
        }
        public string Nombre
        {
            get => nombre;
            set
            {
                nombre = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Nombre)));
            }
        }
        public float ValorUnitario
        {
            get => valor_unitario;
            set
            {
                valor_unitario = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValorUnitario)));
            }
        }
        public int TipoProductosId
        {
            get => tipo_productos_id;
            set
            {
                tipo_productos_id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TipoProductosId)));
            }
        }
        public int UsersId
        {
            get => users_id;
            set
            {
                users_id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UsersId)));
            }
        }
        public int Id
        {
            get => id;
            set
            {
                id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Id)));
            }
        }
        public bool IsCompleted
        {
            get => isCompleted;
            set
            {
                isCompleted = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCompleted)));
            }
        }
        public Command UpdateCommand { get; set; }
        public ICommand ExitCommand { get; }
    }
}