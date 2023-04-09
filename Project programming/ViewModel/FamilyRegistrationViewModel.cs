using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Classes;
using AppService;
using Database;

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

        private Dictionary<string, object> FamilyRegistrationPageData = (App.Current as App).currentData;

        private User User;

        private Family Family;


        private SQLFamilyRepository _familyRepository = new SQLFamilyRepository();

        private SQLUserRepository _userRepository = new SQLUserRepository();
        public FamilyRegistrationViewModel()
        {
            User = FamilyRegistrationPageData["User"] as User;
            JoinToFamily = new Command(async () =>
            {
                if (User.FamilyId != 0)
                {
                    await App.AlertSvc.ShowAlertAsync("", "You are alredy member of group");
                }
                else if (!await _familyRepository.IsExistFamilyAsync(CreatorEmailJoin))
                {
                    await App.AlertSvc.ShowAlertAsync("", "Family with this Email does not exist");
                    CreatorEmailJoin = null;
                }
                else if (await _familyRepository.IsFamilyPasswordCorrectAsync(CreatorEmailJoin, FamilyPasswordJoin))
                {
                    if (!await _userRepository.AddFamilyToUserAsync(CreatorEmailJoin, User.Email))
                    {
                        await App.AlertSvc.ShowAlertAsync("", "Sorry, but something goes wrong and we can not add Family Id");
                    }
                    else
                    {
                        User.ChangeFamilyId(await _familyRepository.GetFamilyIdAsync(_creatorEmaiJoin));
                        Family = new Family(FamilyNameCreation, FamilyPasswordCreation, User.Email);
                        FamilyRegistrationPageData["User"] = User;
                        FamilyRegistrationPageData["Family"] = Family;
                        (App.Current as App).currentData = FamilyRegistrationPageData;
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
                if (User.FamilyId != 0)
                {
                    await App.AlertSvc.ShowAlertAsync("", "You are alredy member of group");
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
                    Family = new Family(FamilyNameCreation, FamilyPasswordCreation, User.Email);
                    if (!await _familyRepository.CreateFamilyAsync(Family))
                    {
                        await App.AlertSvc.ShowAlertAsync("Sorry", "But we can't create family, something goes wrong");
                    }
                    else if (!await _userRepository.AddFamilyToUserAsync(User.Email, User.Email))
                    {
                        await App.AlertSvc.ShowAlertAsync("Sorry", "But we can't add familyId to user");
                    }
                    else
                    {
                        User.ChangeFamilyId(await _familyRepository.GetFamilyIdAsync(User.Email));
                        FamilyRegistrationPageData["User"] = User;
                        FamilyRegistrationPageData["Family"] = Family;
                        (App.Current as App).currentData = FamilyRegistrationPageData;

                        await App.AlertSvc.ShowAlertAsync("Good", "Creation is successful");
                    }

                }
            });
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
