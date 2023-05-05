using PasswordLogic;
using WorkWithEmail;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Classes;
using System.Windows.Input;
using AppService;
using Database;
using DataCollector;

namespace RegistrationPage
{
    public class RegistrationPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SendEmail { get; set; }
        public ICommand ReigistarationButtonIsPressed { get; set; }

        public bool _isVisable = true;

        private SQLUserRepository _userRepository = new SQLUserRepository();

        private SQLFamilyRepository _familyRepository = new SQLFamilyRepository();

        private string _password;

        private string _repeatedPassword;

        private string _name;

        private string _email;

        private int? _answer = null;

        private int? _confirmationCode = null;

        private IAppData _appData = DependencyService.Get<IAppData>();

        private PasswordLog _passwordLog = new PasswordLog();

        public RegistrationPageViewModel()
        {
            SendEmail = new Command(async () =>
            {
                GiveANumberToCode();
                if (!CheckEmailCorectness.ConnectionAvailable())
                {
                    await App.AlertSvc.ShowAlertAsync("Ooops ", "There is no internet, check your connection, please", "ОК ");
                }
                else if (!Password.Equals(RepeatedPassword))
                {
                    await App.AlertSvc.ShowAlertAsync("", $"Password are not equal");
                }
                else if (EmailWriter.SendMessage(Email, "Confirmation Code", "Code :" + _confirmationCode.ToString()))
                {
                    await App.AlertSvc.ShowAlertAsync("Confirmation Code", "We have sent you confirmation code on Email");
                }
                else
                {
                    await App.AlertSvc.ShowAlertAsync("O_o ", "You wrote non-existed Email");
                }
            });

            ReigistarationButtonIsPressed = new Command(async () =>
            {
                IsVisable = false;
                if (await _userRepository.IsUserExistsAsync(Email))
                {
                    await App.AlertSvc.ShowAlertAsync("Attention", $"Account with this Email alredy exist");
                    IsVisable = true;
                }
                else if (Answer.Equals(_confirmationCode))
                {
                    User user = new User(Name, _passwordLog.GetHash(Password), Email);
                    if (await _userRepository.AddUserAsync(user))
                    {

                        await App.AlertSvc.ShowAlertAsync("Great", "You Succesfully registered");
                        await Task.Delay(1000);

                        user = await _userRepository.GetFullPersonInformationAsync(Email);
                        _appData.AddUser(user);
                        ushort FamilyId = user.FamilyId;
                        _appData.AddFamily(await _familyRepository.GetFullFamilyInformationAsync(FamilyId));

                        await Shell.Current.GoToAsync("AccountPageView");
                    }
                    else
                    {
                        await App.AlertSvc.ShowAlertAsync("", "You alreade have account");
                        IsVisable = true;

                        await Shell.Current.GoToAsync("ForgottenPasswordPage");
                    }
                }
                else
                {
                    IsVisable = true;
                    await App.AlertSvc.ShowAlertAsync("", "Confirmation Code should be equal to your answer");
                    Answer = null;
                }
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
                    Registaration();
                }
            }
        }
        private void GiveANumberToCode() => _confirmationCode = PasswordLog.RandomNumberGenerator();
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    Registaration();
                }
            }
        }
        public string RepeatedPassword
        {
            get => _repeatedPassword;
            set
            {
                if (_repeatedPassword != value)
                {
                    _repeatedPassword = value;
                    EmailSender();
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
                    EmailSender();
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
                    EmailSender();
                }
            }
        }
        public int? Answer
        {
            get => _answer;
            set
            {
                if (_answer != value)
                {
                    _answer = value;
                    Registaration();
                }

            }
        }
        public void Registaration([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            ((Command)ReigistarationButtonIsPressed).ChangeCanExecute();
        }
        public void EmailSender([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            ((Command)SendEmail).ChangeCanExecute();
        }
    }
}
