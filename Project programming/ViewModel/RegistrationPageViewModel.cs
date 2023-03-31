using PasswordLogic;
using WorkWithEmail;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Classes;
using System.Windows.Input;
using WorkWithDatabase;
using AppService;

namespace RegistrationPage
{
    public class RegistrationPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _password;

        private string _repeatedPassword;

        private string _name;

        private string _email;

        private int countTry = 0;

        private DateTime? _date = null;
        private int? _answer { get; set; } = null;
        private int? _confirmationCode { get; set; } = null;
        public ICommand SendEmail { get; set; }
        public ICommand ReigistarationButtonIsPressed { get; set; }

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

                if (await DatabaseLogic.IsUserExistsAsync(new User(Name, Password, Email)))
                {
                    await Task.Run(() =>
                    {
                        App.AlertSvc.ShowAlert("Attention", $"Account with this Email alredy exist");
                    });
                }
                else if ((countTry < 3) && !Answer.Equals(_confirmationCode))
                {
                    await Task.Run(() =>
                    {
                        App.AlertSvc.ShowAlert("Attention", $"You have wrote wrong confirmation code, you have {3 - countTry} attempts left ");
                    });
                }
                else if (countTry == 3)
                {
                    SetTheTime();
                    await Task.Run(() =>
                    {
                        App.AlertSvc.ShowAlert("Sorry", "You have used all attepts, you should wait for 3 minutes, then you will be able to get new code");
                    });
                }
                else if (Answer.Equals(_confirmationCode))
                {
                    if (await DatabaseLogic.AddUserAsync(Name, Password, Email))
                    {
                        await Task.Run(() =>
                        {
                            App.AlertSvc.ShowAlert("Great", "You Succesfully registered");
                        });
                        (App.Current as App)._user = await DatabaseLogic.GetFullPersonInformation(Email);
                        (App.Current as App)._family = DatabaseLogic.GetFullFamilyInformation((ushort)(App.Current as App)._user.FamilyId);
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
                    countTry = 0;
                }
            });

        }
        private void SetTheTime() => _date = DateTime.Now.AddMinutes(2);
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
