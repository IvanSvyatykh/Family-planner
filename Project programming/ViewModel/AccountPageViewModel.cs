using System.ComponentModel;
using Classes;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AppService;
using Members;
using System.Collections.ObjectModel;
using Database;
using DataContext;

namespace AccountPage
{
    public class AccountPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand RemoveMember { get; set; }
        public ObservableCollection<FamilyMember> FamilyMembers { get; set; } = new ObservableCollection<FamilyMember>();
        public ObservableCollection<FamilyMember> SelectedMember { get; set; } = new ObservableCollection<FamilyMember>();

        private SQLUserRepository _userRepository = new SQLUserRepository();

        public ObservableCollection<DataPerson> Person { get; set; } = new ObservableCollection<DataPerson>();

        public AccountPageViewModel()
        {
            DataPerson dataPerson;
            if (CurrentDataContext.GetFamily == null)
            {
                dataPerson = new DataPerson(CurrentDataContext.GetUserName, CurrentDataContext.GetUserName, CurrentDataContext.GetUserSalary, null);
            }
            else
            {
                dataPerson = new DataPerson(CurrentDataContext.GetUserName, CurrentDataContext.GetUserName, CurrentDataContext.GetUserSalary, CurrentDataContext.GetFamilyEmail);
            }

            Person.Add(dataPerson);
            List<User> users = _userRepository.GetAllAccountWithFamilyId(CurrentDataContext.GetUserFamailyId);
            if (users != null)
            {
                foreach (var user in users)
                {
                    FamilyMember familyMember = new FamilyMember(user.Name, user.Email, user.FamilyId);
                    FamilyMembers.Add(familyMember);
                }
            }

            RemoveMember = new Command(async () =>
            {

                if (SelectedMember.Count == 0)
                {
                    await Task.Run(() =>
                    {
                        App.AlertSvc.ShowAlert("", "You did not choose anybody from family");
                    });
                }
                else
                {
                    foreach (var user in SelectedMember)
                    {
                        await _userRepository.RemoveMemberOfFamily(user.FamilyId);
                        FamilyMembers.Remove(user);
                    }
                    await Task.Run(() =>
                    {
                        App.AlertSvc.ShowAlert("", "We deleted members");
                    });
                }




            });
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}