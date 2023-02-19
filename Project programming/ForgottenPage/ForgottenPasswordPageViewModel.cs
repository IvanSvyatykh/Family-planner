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
        public string _email { get; set; }
        public ICommand SendEmail { get; set; }

        public ForgottenPasswordPageViewModel()
        {
            SendEmail = new Command(() =>
            {
                int confirmationCode = PasswordLog.RandomNumberGenerator();

                if (!EmailWriter.SendMessage(_email, "confirmationCode code", "Code: " + confirmationCode.ToString()))
                {
                    Application.Current.MainPage.DisplayAlert("O_o", "You  have written a non-existent Email", "Ok");
                    _email = string.Empty;
                }

                int countTry = 0;

                while (countTry != 3)
                {
                    if (int.TryParse(Application.Current.MainPage.
                        DisplayPromptAsync("Confirmation", "Please write here code, which we sent you on Email", "Send", "").ToString(), out int answer))
                    {
                        if (answer != confirmationCode)
                        {
                            countTry++;
                            Application.Current.MainPage.DisplayAlert("Attention", "Check code correctness", "Ok");
                        }
                        else
                        {
                            Application.Current.MainPage.DisplayAlert("Great", "N+ow you can restore your password", "Ok");
                            break;
                        }
                    }
                    else
                    {
                        Application.Current.MainPage.DisplayAlert("Attention", "Check code correctness", "Ok");
                        countTry++;
                    }
                    if (countTry == 2)
                    {
                        Application.Current.MainPage.DisplayAlert("Attention", "You have last attempt, then you will have to wait untill two minutes will be over", "Ok");
                    }
                }
                if (countTry == 3)
                {
                    Application.Current.MainPage.DisplayAlert("Sorry", "You write wrong code three time, wait for two minutes", "Ok");
                }
            });
        }
        public string Email
        {
            get => _email;
            set
            {
                if (CheckEmailCorectness.IsValidEmail(value))
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            ((Command)SendEmail).ChangeCanExecute();
        }
    }
}

