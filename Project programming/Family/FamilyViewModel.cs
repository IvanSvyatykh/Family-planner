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

namespace FamilyPage
{
    public class FamilyViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private static User _user = DatabaseLogic.GetFullPersonInformation((App.Current as App).UserEmail);
        private bool _isFamilyIdEmpty = IsFamilyIdEmpty();
        public ICommand CreateFamily;
        private string _familyName = null;
        private string _familyCode = null;


        public FamilyViewModel()
        {
            CreateFamily = new Command(async () =>
            {


            },
             () => FamilyName != null && FamilyCode != null);



        }

        private static bool IsFamilyIdEmpty() => _user.FamilyId == null;
        public bool IsFamilyEmpty => _isFamilyIdEmpty;
        public string FamilyName
        {
            get => _familyName;
            set
            {
                if (_familyName != value)
                {
                    _familyName = value;
                    OnPropertyChanged();
                }
            }
        }
        public string FamilyCode
        {
            get => _familyCode;
            set
            {
                if (_familyCode != value)
                {
                    _familyCode = value;
                    OnPropertyChanged();
                }
            }
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            ((Command)CreateFamily).ChangeCanExecute();
        }
    }
}
