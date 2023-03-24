using PasswordLogic;
using Project_programming.WorkWithEmail;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Classes;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkWithDatabase;

namespace Project_programming
{
    public class RegistrationPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _password;

        private string _repeatedPassword;

        private string _name;

        private string _email;

        private int countTry = 0;
        private bool _wasPressed = false;

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
                    await Task.Run(async () =>
                    {
                        await Task.Delay(500);
                        App.AlertSvc.ShowAlert("Ooops ", "There is no internet, check your connection, please", "ОК ");
                    });

                }
                else if (!EmailWriter.SendMessage(Email, "Confirmation Code", "Code :" + _confirmationCode.ToString()))
                {
                    await Task.Run(async () =>
                    {
                        await Task.Delay(500);
                        App.AlertSvc.ShowAlert("O_o ", "You wrote non-existed Email", "ОК ");
                    });
                }
                else
                {
                    await Task.Run(async () =>
                    {
                        await Task.Delay(500);
                        App.AlertSvc.ShowAlert("Confirmation Code", "We have sent you confirmation code on Email", "Ok");
                    });
                }
            },
            () => CheckEmailCorectness.IsValidEmail(Email) && (Password == RepeatedPassword));

            ReigistarationButtonIsPressed = new Command(async () =>
            {
                _wasPressed = true;
                if (Answer == null)
                {
                    await Task.Run(async () =>
                    {
                        await Task.Delay(500);
                        App.AlertSvc.ShowAlert("Attention", $"Code can not be null");
                        _wasPressed = false;
                    });
                }
                else if (!CheckEmailCorectness.IsValidEmail(Email))
                {
                    await Task.Run(async () =>
                    {
                        await Task.Delay(500);
                        App.AlertSvc.ShowAlert("Attention", $"You wrote string that can not be a Email");
                        _wasPressed = false;
                    });
                }
                else if (await DatabaseLogic.IsUserExistsAsync(new User(Name, Password, Email)))
                {
                    await Task.Run(async () =>
                    {
                        await Task.Delay(500);
                        App.AlertSvc.ShowAlert("Attention", $"Account with this Email alredy exist");
                        _wasPressed = false;
                    });
                }
                else if ((countTry<3) && !Answer.Equals(_confirmationCode))
                {
                    await Task.Run(async () =>
                    {
                        await Task.Delay(500);
                        App.AlertSvc.ShowAlert("Attention", $"You have wrote wrong confirmation code, you have {3 - countTry} attempts left ", "Ok");
                        _wasPressed = false;
                    });
                }
                else if (countTry == 3)
                {
                    SetTheTime();
                    await Task.Run(async () =>
                    {
                        await Task.Delay(500);
                        App.AlertSvc.ShowAlert("Sorry", "You have used all attepts, you should wait for 3 minutes, then you will be able to get new code");
                    });
                }
                else if (Answer.Equals(_confirmationCode))
                {
                    if (await DatabaseLogic.AddUserAsync(Name, Password, Email))
                    {
                        await Task.Run(async () =>
                        {
                            await Task.Delay(500);
                            App.AlertSvc.ShowAlert("Great", "You Succesfully registered");
                        });
                        (App.Current as App).UserEmail = Email;
                        _wasPressed = false;
                        await Shell.Current.GoToAsync("AccountPageView");
                    }
                    else
                    {
                        await Task.Run(async () =>
                        {
                            await Task.Delay(500);
                            App.AlertSvc.ShowAlert("", "You alreade have account");
                        });
                        _wasPressed = false;
                        await Shell.Current.GoToAsync("ForgottenPasswordPage");
                    }
                    countTry = 0;
                }
            },
           () => !_wasPressed);

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
