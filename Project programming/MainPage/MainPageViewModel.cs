using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Project_programming.WorkWithEmail;
using System.Runtime.CompilerServices;
using WorkWithDatabase;
using Classes;


namespace Project_programming
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ICommand SignIn { get; set; }
        string _email { get; set; }
        string _password { get; set; }

        private int _countTry = 0;
        public MainPageViewModel()
        {
            SignIn = new Command(async () =>
            {
                User user = new User(null, Password, Email);
                if (await DatabaseLogic.IsExistsAsync(user))
                {
                    if (await DatabaseLogic.IsPasswordCorrect(user))
                    {
                        await Task.Delay(500);
                        await Shell.Current.GoToAsync("AccountPageView");
                    }
                    else
                    {
                        await Task.Run(async () =>
                        {
                            await Task.Delay(500);
                            App.AlertSvc.ShowAlert("Ooops", "You write wrong password");
                            _countTry++;    
                        });
                        if(_countTry == 3)
                        {
                            _countTry = 0;
                            await Shell.Current.GoToAsync("ForgottenPasswordPage");
                        }
                    }
                }
                else
                {
                    await Task.Run(async () =>
                    {
                        await Task.Delay(500);
                        App.AlertSvc.ShowAlert("", "We don't have account with this Email");

                    });
                    await Shell.Current.GoToAsync("Registration Page");
                }

            },
            () => CheckEmailCorectness.IsValidEmail(Email) && Password != null);
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