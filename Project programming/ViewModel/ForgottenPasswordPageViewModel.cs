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
        public ICommand SendEmail { get; set; }
        public ICommand Continue { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private SQLUserRepository _userRepository = new SQLUserRepository();

        private SQLFamilyRepository _familyRepository = new SQLFamilyRepository();

        private int? _newPassword = null;

        private string _email;

        private bool _isSend = false;

        private int? _answer = null;

        public bool _isVisable = true;

        private Dictionary<string, object> ForgottenPageData = (App.Current as App).currentData;
        public ForgottenPasswordPageViewModel()
        {
            SendEmail = new Command(async () =>
            {
                GiveANumberToCode();
                if (!CheckEmailCorectness.ConnectionAvailable())
                {
                    await App.AlertSvc.ShowAlertAsync("Ooops ", "There is no internet, check your connection, please");
                }
                else if (!EmailWriter.SendMessage(Email, "New Password", "Password : " + _newPassword.ToString()))
                {
                    await App.AlertSvc.ShowAlertAsync("O_o ", "You wrote non-existed Email");
                }
                else if (await _userRepository.IsUserExistsAsync(Email))
                {
                    _isSend = true;
                    await App.AlertSvc.ShowAlertAsync("Confirmation Code", "We have sent you confirmation code on Email");
                }
                else
                {
                    await App.AlertSvc.ShowAlertAsync("Sorry", "But we cant't find account with this Email");
                }
            });

            Continue = new Command(async () =>
            {
                IsVisable = false;
                if (!Answer.Equals(_newPassword))
                {
                    await App.AlertSvc.ShowAlertAsync("Attention", $"You have wrote wrong confirmation code");
                    IsVisable = true;
                }
                else
                {
                    if (await _userRepository.ChangeUserPasswordAsync(Email, Answer.ToString()))
                    {

                        await App.AlertSvc.ShowAlertAsync("", "You changed your password");
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
                        IsVisable = true;
                        await App.AlertSvc.ShowAlertAsync("", "Something goes wrong");
                    }
                }
            },
            () => _isSend);
        }

        public bool IsVisable
        {
            get => _isVisable;

            set
            {
                if (_isVisable != value)
                {
                    _isVisable = value;
                    ContinueChanged();
                }
            }
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