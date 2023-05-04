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

        public ICommand GetUniqueCategories { get; set; }
        public ObservableCollection<FamilyMember> FamilyMembers { get; set; } = new ObservableCollection<FamilyMember>();
        public ObservableCollection<FamilyMember> SelectedMember { get; set; } = new ObservableCollection<FamilyMember>();
        public ObservableCollection<DataPerson> Person { get; set; } = new ObservableCollection<DataPerson>();

        private SQLUserRepository _userRepository = new SQLUserRepository();

        private SQLFamilyRepository _familyRepository = new SQLFamilyRepository();

        private SQLGoodsCategoriesRepository _categoriesRepository = new SQLGoodsCategoriesRepository();

        private static Dictionary<string, object> _accountPageCurrentData = (App.Current as App).currentData;

        private User _userCurrent = _accountPageCurrentData["User"] as User;

        private Family _family = _accountPageCurrentData["Family"] as Family;

        private bool _isAdmin = false;

        public AccountPageViewModel()
        {
            InitializationFields();

            RemoveMember = new Command(async () =>
            {

                if (SelectedMember.Count == 0)
                {
                    await App.AlertSvc.ShowAlertAsync("", "You did not choose anybody from family");
                }
                else if (!SelectedMember.All(m => m.MemberEmail.Equals(_family.Email)))
                {
                    foreach (var user in SelectedMember)
                    {
                        await _userRepository.RemoveMemberOfFamilyAsync(user.FamilyId, user.Id);
                        FamilyMembers.Remove(user);
                    }
                    SelectedMember.Clear();
                    OnPropertyChanged();
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
                    if (await _familyRepository.RemoveFamily(_family.Id))
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

            GetUniqueCategories = new Command(async () =>
            {
                if (SelectedMember.Count == 0)
                {
                    await App.AlertSvc.ShowAlertAsync("", "You did not choose anybody from family");
                }
                else if (!SelectedMember.All(m => m.MemberEmail.Equals(_userCurrent.Email)))
                {
                    int uniqueCategories = 0;
                    foreach (var user in SelectedMember)
                    {
                        uniqueCategories += await AddNewCategories(user.Id);
                    }

                    await App.AlertSvc.ShowAlertAsync("", $"We added {uniqueCategories} categories ");
                }
                else
                {
                    await App.AlertSvc.ShowAlertAsync("", "You chose your account, you can not add any unique categories");
                    SelectedMember.Clear();
                }
            });
        }

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
        private void InitializationFields()
        {
            if (_family == null)
            {
                Person.Add(new DataPerson(_userCurrent.Name, _userCurrent.Email, null));
            }
            else
            {
                Person.Add(new DataPerson(_userCurrent.Name, _userCurrent.Email, _family.Email));
            }
            FamilyMembers = GetAllFamilyAccount(_userRepository.GetAllAccountWithFamilyId(_userCurrent.FamilyId));

            if (_family != null && Person[0].Email.Equals(_family.Email))
            {
                IsAdmin = true;
            }
        }
        private async Task<int> AddNewCategories(uint userId)
        {
            List<string> existedCategories = _categoriesRepository.GetAllUsersCategoriesName(_userCurrent.Id);
            List<string> newCategories = _categoriesRepository.GetAllUsersCategoriesName(userId).Except(existedCategories).ToList();

            foreach (string category in newCategories)
            {
                await _categoriesRepository.AddCategoryAsync(category, _userCurrent.Id);
            }

            return newCategories.Count();
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}