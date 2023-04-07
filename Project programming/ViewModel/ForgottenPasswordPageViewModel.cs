using PasswordLogic;
using WorkWithEmail;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AppService;
using Database;
using Classes;

namespace ForgottenPasswordPage
{
    public class ForgottenPasswordPageViewModel : INotifyPropertyChanged
    {
        private string _email;
        private bool _isSend = false;
        private int? _answer { get; set; } = null;
        public ICommand SendEmail { get; set; }
        public ICommand Continue { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private SQLUserRepository _userRepository = new SQLUserRepository();
        private SQLFamilyRepository _familyRepository = new SQLFamilyRepository();
        private int? _newPassword { get; set; } = null;

        private Dictionary<string, object> ForgottenPageData = (App.Current as App).currentData;
        public ForgottenPasswordPageViewModel()
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
                else if (!EmailWriter.SendMessage(Email, "New Password", "Password : " + _newPassword.ToString()))
                {
                    await Task.Run(() =>
                    {
                        App.AlertSvc.ShowAlert("O_o ", "You wrote non-existed Email", "ОК ");
                    });
                }
                else if (await _userRepository.IsUserExistsAsync(Email))
                {
                    await Task.Run(() =>
                    {
                        _isSend = true;
                        App.AlertSvc.ShowAlert("Confirmation Code", "We have sent you confirmation code on Email", "Ok");
                    });
                }
                else
                {
                    await Task.Run(() =>
                    {
                        App.AlertSvc.ShowAlert("Sorry", "But we cant't find account with this Email", "Ok");
                    });
                }
            });

            Continue = new Command(async () =>
            {
                if (!Answer.Equals(_newPassword))
                {
                    await Task.Run(() =>
                     {
                         App.AlertSvc.ShowAlert("Attention", $"You have wrote wrong confirmation code");
                     });
                }
                else
                {
                    if (await _userRepository.ChangeUserPasswordAsync(Email, Answer.ToString()))
                    {
                        await Task.Run(() =>
                        {
                            App.AlertSvc.ShowAlert("", "You changed your password");
                        });
                        await Task.Delay(1000);
                        User user = await _userRepository.GetFullPersonInformationAsync(Email);
                        ForgottenPageData.Add("User", user);
                        ushort FamilyId = user.FamilyId;
                        ForgottenPageData.Add("Family", await _familyRepository.GetFullFamilyInformationAsync(FamilyId));
                        (App.Current as App).currentData = ForgottenPageData;
                        await Shell.Current.GoToAsync("AccountPageView");
                    }
                    else
                    {
                        await Task.Run(() =>
                       {
                           App.AlertSvc.ShowAlert("", "Something ggoes wrong");
                       });
                    }

                }
            },
            () => _isSend);
        }
        private void GiveANumberToCode() => _newPassword = PasswordLog.RandomNumberGenerator();
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