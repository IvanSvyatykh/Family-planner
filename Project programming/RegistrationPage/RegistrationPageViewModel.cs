using PageLogic;
using PasswordLogic;
using Project_programming.WorkWithEmail;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
            () => CheckEmailCorectness.IsValidEmail(Email));

            ReigistarationButtonIsPressed = new Command(async () =>
            {
                countTry++;
                if (ForgottenPagePasswordLogic.CheckCountTry(countTry) && !ForgottenPagePasswordLogic.CompareAnswerAndCode(Answer, _confirmationCode))
                {
                    await Task.Run(async () =>
                    {
                        await Task.Delay(500);
                        App.AlertSvc.ShowAlert("Attention", $"You have wrote wrong confirmation code, you have {3 - countTry} attempts left ", "Ok");
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
                else if (ForgottenPagePasswordLogic.CompareAnswerAndCode(Answer, _confirmationCode))
                {
                    if (await DatabaseLogic.AddUserAsync(Name, Password, Email))
                    {
                        await Task.Run(async () =>
                        {
                            await Task.Delay(500);
                            App.AlertSvc.ShowAlert("Great", "You Succesfully registered");
                        });
                        // await Shell.Current.GoToAsync("Account Page");
                    }
                    else
                    {
                        await Task.Run(async () =>
                        {
                            await Task.Delay(500);
                            App.AlertSvc.ShowAlert("", "You alreade have account");

                        });
                        //  await Shell.Current.GoToAsync("ForgottenPasswordPage");
                    }
                    countTry = 0;
                }
            },
           () => CheckEmailCorectness.IsValidEmail(Email) && Answer != null);

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
