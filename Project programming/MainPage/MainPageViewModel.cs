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
        public event PropertyChangedEventHandler? PropertyChanged;
        public ICommand RegistarationPageCommand { get; set; }
        public ICommand ForgottenPasswordPage { get; set; }
        public ICommand SignIn { get; set; }
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
            SignIn = new Command(() =>
            {
                Shell.Current.GoToAsync("SignInPage");
            },
            () => CheckEmailCorectness.IsValidEmail(Email) && Password!=null);
        }
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
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
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            ((Command)SignIn).ChangeCanExecute();
        }

    }
}