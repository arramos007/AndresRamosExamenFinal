using AndresRamosExamenFinal.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using AndresRamosExamenFinal.ViewModel;
using AndresRamosExamenFinal.View;

namespace AndresRamosExamenFinal.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
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
        private INavigation _navigation;
        public MainPageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            IsBusy = true;
            GetGroupedProductoList().ContinueWith(t =>
            {
                IsBusy = false;
                GroupedProductoList = t.Result;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GroupedProductoList)));
            });
            Delete = new Command<ProductoModel>(HandleDelete);
            Logout = new Command(HandleLogout);
            ChangeIsCompleted = new Command<ProductoModel>(HandleChangeIsCompleted);
            AddItem = new Command(HandleAddItem);
            NoteSelectedCommand = new Command<ProductoModel>(async (item) =>
            {
                if (item is null)
                    return;
                var detailViewModel = new DetailProductoPageViewModel
                {
                    CodigoPrincipalProducto = item.codigo_principal_producto,
                    CodigoAuxiliarProducto = item.codigo_auxiliar_producto,
                    Nombre = item.nombre,
                    ValorUnitario = item.valor_unitario,
                    TipoProductosId = item.tipo_productos_id,
                    UsersId = item.users_id,
                    Id = item.id,
                    IsCompleted = item.isCompleted == 0 ? false : true
                };
                await Application.Current.MainPage.Navigation.PushAsync(new DetailProductoPage(detailViewModel));
            });
        }
        public async Task HandleNoteSelected(ProductoModel producto)
        {
            if (producto is null)
                return;

            var detailViewModel = new DetailProductoPageViewModel
            {

                CodigoPrincipalProducto = producto.codigo_principal_producto,
                CodigoAuxiliarProducto = producto.codigo_auxiliar_producto,
                Nombre = producto.nombre,
                ValorUnitario = producto.valor_unitario,
                TipoProductosId = producto.tipo_productos_id,
                UsersId = producto.users_id,
                Id = producto.id,
                IsCompleted = producto.isCompleted == 0 ? false : true
            };
            await Application.Current.MainPage.Navigation.PushAsync(new DetailProductoPage(detailViewModel));
        }
        ProductoModel selectedNote;
        public ProductoModel SelectedNote
        {
            get => selectedNote;
            set
            {
                selectedNote = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedNote)));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public ILookup<string, ProductoModel> GroupedProductoList { get; set; }
        public string Title => "My Producto list";
        private async Task<ILookup<string, ProductoModel>> GetGroupedProductoList()
        {
            return (await App.Database.GetProductoModelsAsync())
                             .OrderBy(t => t.isCompleted)
                             .ToLookup(t => t.isCompleted == 1 ? "Completed" : "Active");
        }
        public Command<ProductoModel> Delete { get; set; }
        public Command Logout { get; set; }
        public Command<ProductoModel> NoteSelectedCommand { get; }
        public async void HandleDelete(ProductoModel itemToDelete)
        {
            IsBusy = true;
            await App.Database.DeleteProductoModelAsync(itemToDelete);
            // Update displayed list
            GroupedProductoList = await GetGroupedProductoList();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GroupedProductoList)));
            IsBusy = false;
        }
        public async void HandleLogout()
        {
            try
            {
                IsBusy = true;
                await App.Database.LogoutProductoModelAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }
        public Command<ProductoModel> ChangeIsCompleted { get; set; }
        public async void HandleChangeIsCompleted(ProductoModel itemToUpdate)
        {
            IsBusy = true;
            await App.Database.ChangeItemIsCompleted(itemToUpdate);
            // Update displayed list
            GroupedProductoList = await GetGroupedProductoList();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GroupedProductoList)));
            IsBusy = false;
        }
        public Command AddItem { get; set; }
        public async void HandleAddItem()
        {
            await _navigation.PushModalAsync(new AddProductoPage());
        }
        public async Task RefreshTaskList()
        {
            IsBusy = true;
            GroupedProductoList = await GetGroupedProductoList();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GroupedProductoList)));
            IsBusy = false;
        }
    }
}
