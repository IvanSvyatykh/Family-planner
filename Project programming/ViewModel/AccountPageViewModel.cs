using System.ComponentModel;
using Classes;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AppService;
using System.Collections.ObjectModel;
using Database;

namespace AccountPage
{
    public class AccountPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand RemoveMember { get; set; }
        public ObservableCollection<FamilyMember> FamilyMembers { get; set; } = new ObservableCollection<FamilyMember>();
        public ObservableCollection<FamilyMember> SelectedMember { get; set; } = new ObservableCollection<FamilyMember>();
        public ObservableCollection<DataPerson> Person { get; set; } = new ObservableCollection<DataPerson>();

        private SQLUserRepository _userRepository = new SQLUserRepository();

        private SQLFamilyRepository _familyRepository = new SQLFamilyRepository();

        private static Dictionary<string, object> AccountPageCurrentData = (App.Current as App).currentData;

        private User User = AccountPageCurrentData["User"] as User;

        private Family Family = AccountPageCurrentData["Family"] as Family;

        private bool _isAdmin = false;

        public AccountPageViewModel()
        {
            DataPerson CurrentUserData = InitializationCurrentUser();
            Person.Add(CurrentUserData);

            FamilyMembers = GetAllFamilyAccount(_userRepository.GetAllAccountWithFamilyId(User.FamilyId));

            if (Family != null && CurrentUserData.Email.Equals(Family.Email))
            {
                IsAdmin = true;
            }

            RemoveMember = new Command(async () =>
            {

                if (SelectedMember.Count == 0)
                {
                    await App.AlertSvc.ShowAlertAsync("", "You did not choose anybody from family");
                }
                else if (!SelectedMember.All(m => m.MemberEmail.Equals(Family.Email)))
                {
                    foreach (var user in SelectedMember)
                    {
                        await _userRepository.RemoveMemberOfFamilyAsync(user.FamilyId, user.Id);
                        FamilyMembers.Remove(user);
                    }

                    await App.AlertSvc.ShowAlertAsync("", "We deleted members");
                }
                else if (await App.AlertSvc.ShowConfirmationAsync("", "You chose creator account, if you delete this account, family will be deleted too. Are you sure?"))
                {

                    foreach (var user in FamilyMembers)
                    {
                        await _userRepository.RemoveMemberOfFamilyAsync(user.FamilyId, user.Id);
                        SelectedMember.Remove(user);
                        OnPropertyChanged();
                    }
                    FamilyMembers.Clear();
                    if (await _familyRepository.RemoveFamily(Family.Id))
                    {
                        await App.AlertSvc.ShowAlertAsync("", "Family was deleted");
                    }
                    else
                    {
                        await App.AlertSvc.ShowAlertAsync("", "Something goes wrong");
                    }

                }
                else
                {
                    SelectedMember.Clear();
                    OnPropertyChanged();
                }
            });

        }

        public bool NotAdmin => !IsAdmin;
        public bool IsAdmin
        {
            get => _isAdmin;

            set
            {
                if (_isAdmin != value)
                {
                    _isAdmin = value;
                    OnPropertyChanged();
                }
            }
        }
        private ObservableCollection<FamilyMember> GetAllFamilyAccount(List<User> users)
        {
            ObservableCollection<FamilyMember> members = new ObservableCollection<FamilyMember>();

            if (users != null)
            {
                foreach (var user in users)
                {
                    FamilyMember familyMember = new FamilyMember(user.Name, user.Email, user.FamilyId, user.Id);
                    members.Add(familyMember);
                }
            }
            return members;

        }

        private DataPerson InitializationCurrentUser()
        {
            DataPerson CurrentUserData;
            if (Family == null)
            {
                CurrentUserData = new DataPerson(User.Name, User.Email, null);
            }
            else
            {
                CurrentUserData = new DataPerson(User.Name, User.Email, Family.Email);
            }
            return CurrentUserData;
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}