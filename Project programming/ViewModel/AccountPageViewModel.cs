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
        private SQLUserRepository _userRepository = new SQLUserRepository();
        public ICommand SaveChangesInSalary { get; set; }
        public ObservableCollection<FamilyMember> FamilyMembers { get; set; } = new ObservableCollection<FamilyMember>();



        public ObservableCollection<DataPerson> Person { get; set; } = new ObservableCollection<DataPerson>();


        public AccountPageViewModel()
        {
            DataPerson dataPerson = new DataPerson((App.Current as App)._user.Name, (App.Current as App)._user.Email, (App.Current as App)._user.Salary.ToString(), (App.Current as App)._family.Email);
            Person.Add(dataPerson);
            List<User> users = _userRepository.GetAllAccountWithFamilyId((App.Current as App)._user.FamilyId);
            if (users != null)
            {

                foreach (var user in users)
                {
                    FamilyMember familyMember;
                    if ((App.Current as App)._user.Email.Equals((App.Current as App)._family.Email))
                    {
                        familyMember = new FamilyMember(user.Name, user.Email, user.Salary.ToString());
                    }
                    else
                    {
                        familyMember = new FamilyMember(user.Name, user.Email, null);
                    }

                    FamilyMembers.Add(familyMember);
                }
            }
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}