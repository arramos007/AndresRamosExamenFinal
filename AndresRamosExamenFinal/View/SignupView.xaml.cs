using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndresRamosExamenFinal.Model;
using AndresRamosExamenFinal.Service;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AndresRamosExamenFinal.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupView : ContentPage
    {
        public SignupView()
        {
            InitializeComponent();
        }

        private async void BtnSignUp_Clicked(object sender, EventArgs e)
        {
            if (!EntPassword.Text.Equals(EntConfirmPassword.Text))
                await DisplayAlert("Password mismatch", "Please check your confirm password", "Cancel");
            else
            {
                var response = await ApiService.RegisterUser(EntName.Text, EntEmail.Text, EntPassword.Text, EntConfirmPassword.Text);
                if (response)
                {
                    await DisplayAlert("Hi", "Your account has been created", "Alright");
                    EntName.Text = EntEmail.Text = EntPassword.Text = EntConfirmPassword.Text = null;
                    await Navigation.PushModalAsync(new LoginView());
                }
                else
                {
                    var result = "";
                    var jsonResult = Preferences.Get("errors", string.Empty);
                    Preferences.Set("errors", string.Empty);
                    JObject jsonParse = JObject.Parse(jsonResult);
                    foreach (JProperty property in jsonParse.Properties())
                    {
                        if (typeof(Newtonsoft.Json.Linq.JArray) == property.Value.GetType())
                        {
                            foreach (var value in property.Value)
                                result += value + "\n";
                        }
                        else
                            result += property.Value.ToString();  
                    }
                    await DisplayAlert("Oops", result, "Cancel");
                }
            }
        }
        private void BtnLogin_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new LoginView());
        }
    }
}