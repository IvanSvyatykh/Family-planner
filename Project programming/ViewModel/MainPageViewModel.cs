using System.ComponentModel;
using System.Windows.Input;
using WorkWithEmail;
using System.Runtime.CompilerServices;
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

        private string _email = string.Empty;

        private string _password = string.Empty;

        private SQLUserRepository _userRepository = new SQLUserRepository();

        private SQLFamilyRepository _familyRepository = new SQLFamilyRepository();

        private Dictionary<string, object> MainPageData = (App.Current as App).currentData;

        private int _countTry = 0;

        private bool _isVisable = true;
        public MainPageViewModel()
        {

            SignIn = new Command(async () =>
            {
                IsVisable = false;
                User user = new User(null, Password, Email);
                if (!CheckEmailCorectness.ConnectionAvailable())
                {
                    await App.AlertSvc.ShowAlertAsync("", "There is no internet, check your connection, please");
                    IsVisable = true;
                }
                else if (await _userRepository.IsUserExistsAsync(user.Email))
                {
                    if (await _userRepository.IsUserPasswordCorrectAsync(user))
                    {                       
                        user = await _userRepository.GetFullPersonInformationAsync(Email);
                        MainPageData.Add("User", user);
                        ushort FamilyId = user.FamilyId;
                        MainPageData.Add("Family", await _familyRepository.GetFullFamilyInformationAsync(FamilyId));
                        (App.Current as App).currentData = MainPageData;
                        await Shell.Current.GoToAsync("AccountPageView");
                    }
                    else
                    {
                        await App.AlertSvc.ShowAlertAsync("", "You write wrong password");
                        _countTry++;
                        IsVisable = true;
                        if (_countTry == 3)
                        {
                            _countTry = 0;
                            await Shell.Current.GoToAsync("ForgottenPasswordPage");
                        }

                    }
                }
                else
                {
                    await App.AlertSvc.ShowAlertAsync("", "We don't have account with this Email");
                    IsVisable = true;
                }


            });



            ForgetPassword = new Command(async () =>
            {
                await Shell.Current.GoToAsync("ForgottenPasswordPage");
            });

            Registration = new Command(async () =>
            {
                await Shell.Current.GoToAsync("RegistrationPage");
            });
        }

        public bool IsVisable
        {
            get => _isVisable;

            set
            {
                if (_isVisable != value) 
                {
                    _isVisable = value;
                    OnPropertyChanged();
                }
            }
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