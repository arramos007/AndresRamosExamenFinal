using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AndresRamosExamenFinal.Model;
using AndresRamosExamenFinal.Service;
using AndresRamosExamenFinal.View;
using AndresRamosExamenFinal.ViewModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AndresRamosExamenFinal.Data
{
    public class ProductoDatabase
    {
        readonly SQLiteAsyncConnection _database;
        int databaseCount = 1;
        public ProductoDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
           // _database.DropTableAsync<ProductoModel>().Wait();
            _database.CreateTableAsync<ProductoModel>().Wait();
        }
        public async Task<List<ProductoModel>> GetProductoModelsAsync()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var deleteProducto = _database.Table<ProductoModel>().Where(i => i.Delete == true);
                var updateProducto = _database.Table<ProductoModel>().Where(i => i.Update == true);
                var storeProducto = _database.Table<ProductoModel>().Where(i => i.Store == true);
                if (await storeProducto.CountAsync() > 0)
                {
                    var producto = storeProducto.ToListAsync();
                    var productostore = JsonConvert.SerializeObject(producto).ToString();
                    JObject productoJson = JObject.Parse(productostore);
                    List<ProductoModel> listStore = new List<ProductoModel> { };
                    foreach (var row in productoJson["Result"])
                    {
                        listStore.Add(new ProductoModel
                        {
                            isCompleted = (int)row["isCompleted"],
                            codigo_principal_producto = (string)row["codigo_principal_producto"],
                            codigo_auxiliar_producto = (string)row["codigo_auxiliar_producto"],
                            nombre = (string)row["nombre"],
                            valor_unitario = (float)row["valor_unitario"],
                            tipo_productos_id = (int)row["tipo_productos_id"],
                            users_id = (int)row["users_id"]
                        });
                    }
                    await storeProducto.DeleteAsync();
                    await ApiService.AllStoreProducto(listStore);
                }

                if (await updateProducto.CountAsync() > 0)
                {
                    var producto = updateProducto.ToListAsync();
                    var productostore = JsonConvert.SerializeObject(producto).ToString();
                    JObject productoJson = JObject.Parse(productostore);
                    List<ProductoModel> listStore = new List<ProductoModel> { };
                    foreach (var row in productoJson["Result"])
                    {
                        listStore.Add(new ProductoModel
                        {
                            id = (int)row["id"],
                            isCompleted = (int)row["isCompleted"],
                            codigo_principal_producto = (string)row["codigo_principal_producto"],
                            codigo_auxiliar_producto = (string)row["codigo_auxiliar_producto"],
                            nombre = (string)row["nombre"],
                            valor_unitario = (float)row["valor_unitario"],
                            tipo_productos_id = (int)row["tipo_productos_id"],
                            users_id = (int)row["users_id"]
                        });
                    }
                    await storeProducto.DeleteAsync();
                    await ApiService.AllUpdateProducto(listStore);
                }

                if (await deleteProducto.CountAsync() > 0)
                {
                    var producto = deleteProducto.ToListAsync();
                    var productodelete = JsonConvert.SerializeObject(producto).ToString();
                    JObject productoJson = JObject.Parse(productodelete);
                    List<int> deleteId = new List<int> { };
                    foreach (var row in productoJson["Result"])
                    {
                        deleteId.Add((int)row["id"]);
                    }
                    await deleteProducto.DeleteAsync();
                    await ApiService.AllDeleteProducto(deleteId);
                }

                var server = await ApiService.GetProductos();
                var client = await _database.Table<ProductoModel>().ToListAsync();
                var result = "";
                var jsonResult = Preferences.Get("getProducto", string.Empty);
                var clientjsonResult = JsonConvert.SerializeObject(client).ToString();
                JArray jsonParse = JArray.Parse(jsonResult);
                JArray clientJson = JArray.Parse(clientjsonResult);
                List<int> serverId = new List<int> { };
                List<int> clientId = new List<int> { };
                List<int> clientDeletedId = new List<int> { };
                foreach (var row in clientJson)
                {
                    clientId.Add((int)row["id"]); ;
                }
                foreach (var jp in jsonParse)
                {
                    var producto = new ProductoModel
                    {
                        codigo_principal_producto = (string)jp["codigo_principal_producto"],
                        codigo_auxiliar_producto = (string)jp["codigo_auxiliar_producto"],
                        nombre = (string)jp["nombre"],
                        valor_unitario = (float)jp["valor_unitario"],
                        tipo_productos_id = (int)jp["tipo_productos_id"],
                        users_id = (int)jp["users_id"],
                        isCompleted = (int)jp["isComplete"],
                        id = (int)jp["id"],
                        created_at = (DateTime)jp["created_at"],
                        updated_at = (DateTime)jp["updated_at"]
                    };
                    if (!client.Any(x => x.id == (int)jp["id"]))
                    {
                        await _database.InsertAsync(producto);
                    }
                    else if (client.Any(x => x.id == (int)jp["id"]))
                    {
                        JObject updateClient = JObject.Parse(JsonConvert.SerializeObject(client.Single(x => x.id == (int)jp["id"])));
                        producto.cid = (int)updateClient["cid"];
                        await _database.UpdateAsync(producto);
                    }
                    serverId.Add((int)jp["id"]);
                }
                IEnumerable<int> res = clientId.AsQueryable().Except(serverId);
                foreach (int id in res)
                {
                    await DeleteProductoModelAsync(new ProductoModel { id = id });
                }
            }
            return await _database.Table<ProductoModel>().Where(i => i.Delete == false).ToListAsync();
        }

        public async Task InitializeProducto()
        {
        }
        public Task<ProductoModel> GetProductoModelAsync(int id)
        {
            return _database.Table<ProductoModel>()
                            .Where(i => i.id == id)
                            .FirstOrDefaultAsync();
        }
        public async Task<int> SaveProductoModelAsync(ProductoModel note)
        {
            try
            {
                if (note.id != 0)
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        Thread.Sleep(50);
                        var producto = await ApiService.PutProducto(note);
                        return await _database.UpdateAsync(producto);
                    }
                    else
                    {
                        note.Update = true;
                        note.updated_at = DateTimeOffset.Now;
                        Thread.Sleep(50);
                        return await _database.UpdateAsync(note);
                    }
                }
                else
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        Thread.Sleep(50);
                        var producto = await ApiService.PostProducto(note);
                        var result = await _database.InsertAsync(producto);
                        return result;
                    }
                    else
                    {
                        note.Store = true;
                        Thread.Sleep(50);
                        return await _database.InsertAsync(note); ;
                    }
                }
            }
            catch (ArgumentOutOfRangeException outOfRange)
            {
                throw;
            }
        }

        public async Task<int> DeleteProductoModelAsync(ProductoModel note)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    Thread.Sleep(50);
                    await _database.DeleteAsync(note);
                    await ApiService.DeleteProductos(note);
                    return 1;
                }
                else
                {
                    note.Delete = true;
                    Thread.Sleep(50);
                    return await _database.UpdateAsync(note);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task<bool> LogoutProductoModelAsync()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var producto = await ApiService.LogoutAccount();
                Application.Current.MainPage = new NavigationPage(new SignupView());
                return true;
            }
            else
            {
                Preferences.Set("access_token", string.Empty);
                Preferences.Set("userId", string.Empty);
                Preferences.Set("userName", string.Empty);
                return true;
            }
            // (App.Current.MainPage.BindingContext as MainPageViewModel).RefreshTaskList();
        }
        public async Task ChangeItemIsCompleted(ProductoModel itemToChange)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (itemToChange.isCompleted == 0)
                {
                    itemToChange.isCompleted = 1;
                }
                else
                {
                    itemToChange.isCompleted = 0;
                }
                var producto = await ApiService.PutProducto(itemToChange);
                await _database.UpdateAsync(itemToChange);

            }
            else
            {
                if (itemToChange.isCompleted == 0)
                {
                    itemToChange.isCompleted = 1;
                }
                else
                {
                    itemToChange.isCompleted = 0;
                }

                itemToChange.Update = true;
                itemToChange.updated_at = DateTimeOffset.Now;
                await _database.UpdateAsync(itemToChange);
            }

        }
    }
}
