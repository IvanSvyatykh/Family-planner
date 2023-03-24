using Project_programming;
using Project_programming.Model.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkWithDatabase;
using Classes;
using System.Runtime.CompilerServices;

namespace AccountViewModel
{
    public class AccountPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static User _user { get; set; } = DatabaseLogic.GetFullPersonInformation((App.Current as App).UserEmail);
        private static Family _family { get; set; } = DatabaseLogic.GetFullFamilyInformation((ushort)_user.FamilyId);

        public AccountPageViewModel()
        {

        }
        public string Name => "Welcome, " + _user.Name;
        public string Email => _user.Email;
        public string FamilyName
        {
            get
            {             
                if(_family.Name == null)
                {
                    return null;
                }
                else
                {
                    return "Family Name : " + _family.Name;
                }
                

            }
        }
        public string Salary
        {
            get => _user.Salary.ToString();
            set
            {
                if (value != _user.Salary.ToString())
                {
                    _user.Salary = uint.Parse(value);
                    OnPropertyChanged();
                }
            }
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
