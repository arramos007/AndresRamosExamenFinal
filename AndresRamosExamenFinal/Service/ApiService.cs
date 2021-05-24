using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AndresRamosExamenFinal.Model;
using Xamarin.Essentials;

namespace AndresRamosExamenFinal.Service
{
    public class ApiService
    {
        public static async Task<bool> RegisterUser(string name = "", string email = "", string password = "", string password_confirmation = "")
        {
            var register = new RegisterModel()
            {
                name = name,
                email = email,
                password = password,
                password_confirmation = password_confirmation
            };

            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(register);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/register", content);
            if (!response.IsSuccessStatusCode)
            {
                var jsonResult = await response.Content.ReadAsStringAsync();


                Preferences.Set("errors", jsonResult);
                return false;
            }


            return true;
        }
        public static async Task<bool> LoginUser(string email, string password)
        {
            var login = new LoginModel()
            {
                email = email,
                password = password
            };
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/login", content);
            if (!response.IsSuccessStatusCode) return false;

            var jsonResult = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<TokenModel>(jsonResult);
            Preferences.Set("access_token", result.access_token);
            Preferences.Set("userId", result.user.id.ToString());
            Preferences.Set("userName", result.user.name);
            return true;
        }



        public static async Task<bool> AllUpdateProducto(List<ProductoModel> producto)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("access_token", string.Empty));
            var json = JsonConvert.SerializeObject(producto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/productos/all/update", content);
            var jsonResult = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return false;



            return true;
        }


        public static async Task<bool> AllStoreProducto(List<ProductoModel> producto)
        {


            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("access_token", string.Empty));
            var json = JsonConvert.SerializeObject(producto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/productos/all/store", content);
            var jsonResult = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }

        public static async Task<ProductoModel> PostProducto(ProductoModel producto)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("access_token", string.Empty));
            var json = JsonConvert.SerializeObject(producto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/productos", content);
            var jsonResult = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return JsonConvert.DeserializeObject<ProductoModel>("[]");



            return JsonConvert.DeserializeObject<ProductoModel>(jsonResult);
        }


        public static async Task<bool> AllDeleteProducto(List<int> producto)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("access_token", string.Empty));
            var json = JsonConvert.SerializeObject(producto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/productos/all/delete", content);
            var jsonResult = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return false;



            return true;
        }
        public static async Task<ProductoModel> PutProducto(ProductoModel producto)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("access_token", string.Empty));
            var json = JsonConvert.SerializeObject(producto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(AppSettings.ApiUrl + "api/productos/" + producto.id, content);
            var jsonResult = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return JsonConvert.DeserializeObject<ProductoModel>(jsonResult);



            return JsonConvert.DeserializeObject<ProductoModel>(jsonResult);
        }
        public static async Task<List<ProductoModel>> GetProductos()
        {



            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("access_token", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "api/productos");
            Preferences.Set("getProducto", response);
            return JsonConvert.DeserializeObject<List<ProductoModel>>(response);













        }

        public static async Task<bool> DeleteProductos(ProductoModel producto)
        {
            Console.WriteLine("[producto id]");
            Console.WriteLine(producto.id);
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("access_token", string.Empty));
            var response = await httpClient.DeleteAsync(AppSettings.ApiUrl + "api/productos/" + producto.id);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<bool> LogoutAccount()
        {

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("access_token", string.Empty));
            var response = await httpClient.GetAsync(AppSettings.ApiUrl + "api/logout");
            if (!response.IsSuccessStatusCode) return false;

            Preferences.Set("access_token", string.Empty);
            Preferences.Set("userId", string.Empty);
            Preferences.Set("userName", string.Empty);



            return true;
        }
    }
}
