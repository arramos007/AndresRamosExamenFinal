using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AndresRamosExamenFinal.Service;
using Xamarin.Forms;

namespace AndresRamosExamenFinal.ViewModel
{

    public class LoginViewModel : INotifyPropertyChanged
    {
        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Email)));
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
            }
        }

        public LoginViewModel()
        {
            Login = new Command(HandleLoginAsync);
        }

        public async void HandleLoginAsync()
        {
            IsBusy = true;
            var response = await ApiService.LoginUser(Email, Password);
            if (response)
            {
                IsBusy = false;
                try
                {
                    Application.Current.MainPage = new NavigationPage(new MainPage());
                }
                finally{}
            }
            else
            {
                IsBusy = false;
                try
                {
                    await Application.Current.MainPage.DisplayAlert("Oops", "Something went wrong", "Cancel");
                }
                finally { }
            }
        }

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
        public ICommand Login { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}