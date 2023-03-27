using System.ComponentModel;
using System.Windows.Input;
using WorkWithEmail;
using System.Runtime.CompilerServices;
using WorkWithDatabase;
using AppService;
using Classes;
using Database;

namespace MainPage
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SignIn { get; set; }
        public ICommand Registration { get; set; }
        public ICommand ForgetPassword { get; set; }
        private string _email { get; set; }
        private string _password { get; set; }

        private int _countTry = 0;
        public MainPageViewModel()
        {
            SignIn = new Command(async () =>
            {
                User user = new User(null, Password, Email);
                if (!CheckEmailCorectness.ConnectionAvailable())
                {
                    await Task.Run(() =>
                    {
                        App.AlertSvc.ShowAlert("", "There is no internet, check your connection, please");
                    });

                }
                else if (!CheckEmailCorectness.IsValidEmail(Email))
                {
                    await Task.Run(() =>
                    {
                        App.AlertSvc.ShowAlert("", "This string can not be Email");
                    });
                }
                else if (await DatabaseLogic.IsUserExistsAsync(user))
                {
                    if (await DatabaseLogic.IsUserPasswordCorrectAsync(user))
                    {

                        (App.Current as App)._user = await DatabaseLogic.GetFullPersonInformation(Email);
                        (App.Current as App)._family = DatabaseLogic.GetFullFamilyInformation((ushort)(App.Current as App)._user.FamilyId);
                        await Task.Delay(1000);
                        await Shell.Current.GoToAsync("AccountPageView");
                    }
                    else
                    {
                        await Task.Run(() =>
                        {
                            App.AlertSvc.ShowAlert("", "You write wrong password");
                            _countTry++;
                        });
                        if (_countTry == 3)
                        {
                            _countTry = 0;
                            await Shell.Current.GoToAsync("ForgottenPasswordPage");
                        }
                    }
                }
                else
                {
                    await Task.Run(() =>
                    {
                        App.AlertSvc.ShowAlert("", "We don't have account with this Email");
                    });
                    await Shell.Current.GoToAsync("RegistrationPage");
                }

            },
            () => Password != null);

            ForgetPassword = new Command(async () =>
            {
                await Shell.Current.GoToAsync("ForgottenPasswordPage");
            });

            Registration = new Command(async () =>
            {
                await Shell.Current.GoToAsync("RegistrationPage");
            });
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