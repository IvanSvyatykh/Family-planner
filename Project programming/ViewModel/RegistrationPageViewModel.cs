using PasswordLogic;
using WorkWithEmail;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Classes;
using System.Windows.Input;
using AppService;
using Database;

namespace RegistrationPage
{
    public class RegistrationPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _password;

        private string _repeatedPassword;

        private string _name;

        private string _email;
        private int? _answer { get; set; } = null;
        private int? _confirmationCode { get; set; } = null;
        public ICommand SendEmail { get; set; }
        public ICommand ReigistarationButtonIsPressed { get; set; }

        private SQLUserRepository _userRepository = new SQLUserRepository();
        private SQLFamilyRepository _familyRepository = new SQLFamilyRepository();

        public RegistrationPageViewModel()
        {
            SendEmail = new Command(async () =>
            {
                GiveANumberToCode();
                if (!CheckEmailCorectness.ConnectionAvailable())
                {
                    await Task.Run(() =>
                    {
                        App.AlertSvc.ShowAlert("Ooops ", "There is no internet, check your connection, please", "ОК ");
                    });

                }
                else if (!Password.Equals(RepeatedPassword))
                {
                    await Task.Run(() =>
                    {
                        App.AlertSvc.ShowAlert("", $"Password are not equal");
                    });
                }
                else if (EmailWriter.SendMessage(Email, "Confirmation Code", "Code :" + _confirmationCode.ToString()))
                {
                    await Task.Run(() =>
                    {
                        App.AlertSvc.ShowAlert("Confirmation Code", "We have sent you confirmation code on Email");
                    });

                }
                else
                {
                    await Task.Run(() =>
                    {
                        App.AlertSvc.ShowAlert("O_o ", "You wrote non-existed Email");
                    });
                }
            });

            ReigistarationButtonIsPressed = new Command(async () =>
            {

                if (await _userRepository.IsUserExistsAsync(new User(Name, Password, Email)))
                {
                    await Task.Run(() =>
                    {
                        App.AlertSvc.ShowAlert("Attention", $"Account with this Email alredy exist");
                    });
                }
                else if (Answer.Equals(_confirmationCode))
                {
                    User user = new User(Name, Password, Email);
                    if (await _userRepository.AddUserAsync(user))
                    {
                        await Task.Run(() =>
                        {
                            App.AlertSvc.ShowAlert("Great", "You Succesfully registered");
                        });
                        (App.Current as App)._family = await _familyRepository.GetFullFamilyInformationAsync((ushort)(App.Current as App)._user.FamilyId);
                        (App.Current as App)._user = await _userRepository.GetFullPersonInformationAsync(Email);
                        await Task.Delay(1000);
                        await Shell.Current.GoToAsync("AccountPageView");
                    }
                    else
                    {
                        await Task.Run(() =>
                        {
                            App.AlertSvc.ShowAlert("", "You alreade have account");
                        });
                        await Shell.Current.GoToAsync("ForgottenPasswordPage");
                    }
                }
                else
                {
                    await Task.Run(() =>
                    {
                        App.AlertSvc.ShowAlert("", "Confirmation Code should be equal to your answer");
                        Answer = null;
                    });
                }
            });

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
