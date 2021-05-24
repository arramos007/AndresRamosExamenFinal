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
            { id = Id, codigo_principal_producto = Codigo_principal_producto, 
                isCompleted = IsCompleted == true ? 1 : 0 });

            await Application.Current.MainPage.Navigation.PopAsync();
        }
        string codigo_principal_producto;

        string codigo_auxiliar_producto;
        public string Codigo_principal_producto
        {
            get => codigo_principal_producto;
            set
            {
                codigo_principal_producto = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Codigo_principal_producto)));
            }
        }

        public string Codigo_auxiliar_producto
        {
            get => codigo_auxiliar_producto;
            set
            {
                codigo_auxiliar_producto = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(codigo_auxiliar_producto)));
            }
        }

        int id;
        public int Id
        {
            get => id;
            set
            {
                id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Id)));
            }
        }

        bool isCompleted;
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
