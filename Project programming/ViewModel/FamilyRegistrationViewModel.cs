using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Classes;
using AppService;
using Database;
using DataCollector;
using PasswordLogic;
using WorkWithEmail;

namespace FamilyRegistrationPage
{
    public class FamilyRegistrationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand CreateFamily { get; set; }
        public ICommand JoinToFamily { get; set; }

        private string _familyNameCreation = null;

        private string _familyPasswordJoin = null;

        private string _creatorEmaiJoin = null;

        private string _familyPasswordCreation = null;

        private string _repeatedFamilyPasswordCreation = null;

        private IAppData _appData = DependencyService.Get<IAppData>();

        private User _user;

        private Family _family;

        private SQLFamilyRepository _familyRepository = new SQLFamilyRepository();

        private SQLUserRepository _userRepository = new SQLUserRepository();

        private PasswordLog _passwordLog = new PasswordLog();
        public FamilyRegistrationViewModel()
        {
            _user = _appData.User;
            JoinToFamily = new Command(async () =>
            {
                if (!CheckEmailCorectness.ConnectionAvailable())
                {
                    await App.AlertSvc.ShowAlertAsync("", "There is no internet, check your connection, please");
                }
                else if (_user.FamilyId != 0)
                {
                    await App.AlertSvc.ShowAlertAsync("", "You are alredy member of family");
                }

                else if (!await _familyRepository.IsExistFamilyAsync(CreatorEmailJoin))
                {
                    await App.AlertSvc.ShowAlertAsync("", "Family with this Email does not exist");
                    CreatorEmailJoin = null;
                }
                else if (await _familyRepository.IsFamilyPasswordCorrectAsync(CreatorEmailJoin, _passwordLog.GetHash(FamilyPasswordJoin)))
                {
                    if (!await _userRepository.AddFamilyToUserAsync(CreatorEmailJoin, _user.Email))
                    {
                        await App.AlertSvc.ShowAlertAsync("", "Sorry, but something goes wrong and we can not add Family Id");
                    }
                    else
                    {
                        _user.ChangeFamilyId(await _familyRepository.GetFamilyIdAsync(_creatorEmaiJoin));
                        _family = new Family(FamilyNameCreation, FamilyPasswordCreation, CreatorEmailJoin);
                        SaveAppData();
                        CreatorEmailJoin = FamilyPasswordJoin = null;

                        App.AlertSvc.ShowAlert("Great", "You successfully connect to family");
                    }
                }
                else
                {
                    await App.AlertSvc.ShowAlertAsync("", "Password isn't correct");
                }
            });

            CreateFamily = new Command(async () =>
            {
                if (!CheckEmailCorectness.ConnectionAvailable())
                {
                    await App.AlertSvc.ShowAlertAsync("", "There is no internet, check your connection, please");
                }
                else if (_user.FamilyId != 0)
                {
                    await App.AlertSvc.ShowAlertAsync("", "You are alredy member of family");
                }
                else if (!FamilyPasswordCreation.Equals(RepeatedFamilyPasswordCreation))
                {
                    await App.AlertSvc.ShowAlertAsync("", "Password and repeted password are not equal");
                    RepeatedFamilyPasswordCreation = null;
                    FamilyPasswordCreation = null;
                }
                else if (FamilyNameCreation == null || FamilyNameCreation == "")
                {
                    await App.AlertSvc.ShowAlertAsync("Sorry", "Name of Family can not be null");
                }
                else
                {
                    _family = new Family(FamilyNameCreation, _passwordLog.GetHash(FamilyPasswordCreation), _user.Email);
                    if (!await _familyRepository.CreateFamilyAsync(_family))
                    {
                        await App.AlertSvc.ShowAlertAsync("Sorry", "But we can't create family, something goes wrong");
                    }
                    else if (!await _userRepository.AddFamilyToUserAsync(_user.Email, _user.Email))
                    {
                        await App.AlertSvc.ShowAlertAsync("Sorry", "But we can't add familyId to user");
                    }
                    else
                    {
                        _user.ChangeFamilyId(await _familyRepository.GetFamilyIdAsync(_user.Email));
                        SaveAppData();
                        RepeatedFamilyPasswordCreation = FamilyPasswordCreation = FamilyNameCreation = null;

                        await App.AlertSvc.ShowAlertAsync("Good", "Creation is successful");
                    }

                }
            });
        }

        public void SaveAppData()
        {
            _appData.AddUser(_user);
            _appData.AddFamily(_family);
        }

        public string RepeatedFamilyPasswordCreation
        {
            get => _repeatedFamilyPasswordCreation;
            set
            {
                if (_repeatedFamilyPasswordCreation != value)
                {
                    _repeatedFamilyPasswordCreation = value;
                    FamilyCreation();
                }
            }
        }

        public string FamilyPasswordCreation
        {
            get => _familyPasswordCreation;
            set
            {
                if (_familyPasswordCreation != value)
                {
                    _familyPasswordCreation = value;
                    FamilyCreation();
                }
            }
        }

        public string CreatorEmailJoin
        {
            get => _creatorEmaiJoin;
            set
            {
                if (_creatorEmaiJoin != value)
                {
                    _creatorEmaiJoin = value;
                    JoiningToFamily();
                }
            }
        }

        public string FamilyNameCreation
        {
            get => _familyNameCreation;
            set
            {
                if (_familyNameCreation != value)
                {
                    _familyNameCreation = value;
                    FamilyCreation();
                }
            }
        }

        public string FamilyPasswordJoin
        {
            get => _familyPasswordJoin;
            set
            {
                if (_familyPasswordJoin != value)
                {
                    _familyPasswordJoin = value;
                    FamilyCreation();
                }
            }
        }

        public void FamilyCreation([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            ((Command)CreateFamily).ChangeCanExecute();

        }

        public void JoiningToFamily([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            ((Command)JoinToFamily).ChangeCanExecute();

        }
    }
}