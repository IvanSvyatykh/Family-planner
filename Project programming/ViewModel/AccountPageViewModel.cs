using System.ComponentModel;
using Classes;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AppService;
using Members;
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

        private static Dictionary<string, object> AccountPageCurrentData = (App.Current as App).currentData;

        private User User = AccountPageCurrentData["User"] as User;

        private Family Family = AccountPageCurrentData["Family"] as Family;

        public AccountPageViewModel()
        {
            DataPerson CurrentUserData = InitializationCurrentUser();
            Person.Add(CurrentUserData);

            FamilyMembers = GetAllFamilyAccount(_userRepository.GetAllAccountWithFamilyId(User.FamilyId));

            RemoveMember = new Command(async () =>
            {

                if (SelectedMember.Count == 0)
                {
                    await Task.Run(async () =>
                    {
                       await App.AlertSvc.ShowAlertAsync("", "You did not choose anybody from family");
                    });
                }
                else if (!SelectedMember.All(m => m.MemberEmail.Equals(Family.Email)))
                {
                    foreach (var user in SelectedMember)
                    {
                        if (Family.Email.Equals(user.MemberEmail))
                        {

                        }
                        await _userRepository.RemoveMemberOfFamilyAsync(user.FamilyId);
                        FamilyMembers.Remove(user);
                    }
                    await Task.Run(() =>
                    {
                        App.AlertSvc.ShowAlert("", "We deleted members");
                    });
                }
                else if(await App.AlertSvc.ShowConfirmationAsync("" , "You chose creator account, if you delete this account, family will be deleted to"))
                {

                }




            });
        }

        private ObservableCollection<FamilyMember> GetAllFamilyAccount(List<User> users)
        {
            ObservableCollection<FamilyMember> members = new ObservableCollection<FamilyMember>();

            if (users != null)
            {
                foreach (var user in users)
                {
                    FamilyMember familyMember = new FamilyMember(user.Name, user.Email, user.FamilyId);
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
                CurrentUserData = new DataPerson(User.Name, User.Email, User.Salary, null);
            }
            else
            {
                CurrentUserData = new DataPerson(User.Name, User.Email, User.Salary, Family.Email);
            }
            return CurrentUserData;
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}