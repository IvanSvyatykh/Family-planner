﻿using PageLogic;
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
        private string _email;

        private DateTime? _date = null;
        private int? _answer { get; set; } = null;
        public ICommand SendEmail { get; set; }
        public ICommand Continue { get; set; }

        private int countTry = 0;

        public event PropertyChangedEventHandler PropertyChanged;
        private int? _confirmationCode { get; set; } = null;
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
                        App.AlertSvc.ShowAlert("Confirmation Code", "We have sent you confirmation code on Email", "Ok");
                    });
                }
            },
            () => IsEmailCorrect);

            Continue = new Command(async () =>
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
                    await Task.Run(async () =>
                    {
                        await Task.Delay(500);
                        App.AlertSvc.ShowAlert("", "Now you can change your password", "Ok");
                    });
                    countTry = 0;
                }
            },
            () => IsEmailCorrect && !ForgottenPagePasswordLogic.CheckTheTime(_date) && Answer != null);
        }
        private void SetTheTime() => _date = DateTime.Now.AddMinutes(2);
        private void GiveANumberToCode() => _confirmationCode = PasswordLog.RandomNumberGenerator();
        public bool IsEmailCorrect => CheckEmailCorectness.IsValidEmail(Email);   
        
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    EmailChanged();
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
                    ContinueChanged();
                }
            }
        }

        public void EmailChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            ((Command)SendEmail).ChangeCanExecute();
        }
        public void ContinueChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            ((Command)Continue).ChangeCanExecute();
        }
    }
}