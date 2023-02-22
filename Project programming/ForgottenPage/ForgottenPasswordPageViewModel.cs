using PageLogic;
using PasswordLogic;
using Project_programming.WorkWithEmail;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Project_programming.ForgottenPage
{
    public class ForgottenPasswordPageViewModel : INotifyPropertyChanged
    {
        private string _email { get; set; }

        private bool _sendMessegeTrigger = false;
        private int? _answer { get; set; } = null;
        public ICommand SendEmail { get; set; }
        public ICommand Continue { get; set; }

        private int countTry = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        private int _confirmationCode = 0;
        public ForgottenPasswordPageViewModel()
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
                        App.AlertSvc.ShowAlert("Confirmation Code", "We have sent you confirmation code on Email, write it here", "Ok");
                        _sendMessegeTrigger = true;
                    });
                }
            },
            () => CheckEmailCorectness.IsValidEmail(Email));

            Continue = new Command(async () =>
            {

                await Task.Run(async () =>
                {
                    await Task.Delay(500);
                    App.AlertSvc.ShowAlert("", "We have sent you confirmation code on Email", "Ok");
                });

            },
            () => ForgottenPagePasswordLogic.CompareAnswerAndCode(Answer, _confirmationCode) && _sendMessegeTrigger);
        }

        private void GiveANumberToCode() => _confirmationCode = PasswordLog.RandomNumberGenerator();
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
        public int? Answer
        {
            get => _answer;
            set
            {
                if (_answer != value)
                {
                    _answer = value;
                    OnPropertyChanged();
                }
            }
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            ((Command)SendEmail).ChangeCanExecute();
        }
    }
}

