using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Project_programming.WorkWithEmail;
using System.Runtime.CompilerServices;


namespace Project_programming
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand RegistarationPageCommand { get; set; }
        public ICommand ForgottenPasswordPage { get; set; }
        public ICommand SignInIsPressed { get; set; }
        string _email { get; set; }
        string _password { get; set; }
        public MainPageViewModel()
        {
            RegistarationPageCommand = new Command(() =>
            {
                Shell.Current.GoToAsync("RegistrationPage");
            });
            ForgottenPasswordPage = new Command(() =>
            {
                Shell.Current.GoToAsync("ForgottenPasswordPage");
            });
            SignInIsPressed = new Command(() =>
            {
                if (!CheckEmailCorectness.ConnectionAvailable())
                {
                    Application.Current.MainPage.DisplayAlert("Attention", "There is no internet connection", "Ok");
                    return;
                }
                if (_password == null || _email == null)
                {
                    Application.Current.MainPage.DisplayAlert("Attention", "All fields must be field", "Ok");
                    return;
                }
                else
                {
                    Shell.Current.GoToAsync("SignInPage");
                }
            });
        }
        public string Password
        {
            get => _password;
            set
            {
                if (value != null)
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                if (value != null)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}