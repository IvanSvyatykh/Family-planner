using System.ComponentModel;
using WorkWithDatabase;
using Classes;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AppService;
using Members;
using System.Collections.ObjectModel;

namespace AccountPage
{
    public class AccountPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SaveChangesInSalary { get; set; }
        public ObservableCollection<FamilyMember> FamilyMembers { get; set; } = new ObservableCollection<FamilyMember>();

        private List<User> users = DatabaseLogic.GetAllAccountWithFamilyId((App.Current as App)._user.FamilyId);
        public AccountPageViewModel()
        {
            foreach (var user in users)
            {
                FamilyMember familyMember = new FamilyMember(user.Name, user.Email, user.Salary.ToString());
                FamilyMembers.Add(familyMember);
            }


            SaveChangesInSalary = new Command(async () =>
            {
                if (uint.TryParse(Salary, out _))
                {
                    await DatabaseLogic.AddSalaryToUserAsync(Email, uint.Parse(Salary));
                    await Task.Run(() =>
                    {
                        App.AlertSvc.ShowAlert("", "Salary succefully changed");
                    });

                }
                else
                {
                    Salary = (App.Current as App)._user.Salary.ToString();
                    await Task.Run(() =>
                    {
                        App.AlertSvc.ShowAlert("", "Salary must be integer");
                    });
                }

            });
        }
        public string Name => "Welcome, " + (App.Current as App)._user.Name;
        public string Email => (App.Current as App)._user.Email;
        public string FamilyName
        {
            get
            {
                if ((App.Current as App)._family == null)
                {
                    return null;
                }
                else
                {
                    return "Family Name : " + (App.Current as App)._family.Name;
                }


            }
        }
        public string Salary
        {
            get => (App.Current as App)._user.Salary.ToString();
            set
            {
                if (!(App.Current as App)._user.Salary.Equals(value))
                {
                    if (uint.TryParse(value, out uint result))
                    {
                        (App.Current as App)._user.Salary = result;
                        OnPropertyChanged();
                    }
                    else if (value == "" || value == null)
                    {
                        (App.Current as App)._user.Salary = 0;
                        OnPropertyChanged();
                    }
                    else
                    {
                        Salary = (App.Current as App)._user.Salary.ToString();
                        OnPropertyChanged();
                    }
                }

            }
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
