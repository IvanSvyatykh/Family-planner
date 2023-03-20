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
using Families;
using Classes;
using System.Runtime.CompilerServices;

namespace FamilyPage
{
    public class FamilyViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private static User _user =  DatabaseLogic.GetFullPersonInformation((App.Current as App).UserEmail);
        private static Family _family = null;
        private bool _isFamilyIdEmpty = IsFamilyIdEmpty();
        public ICommand CreateFamily;
        public ICommand JoinToFamily;
        private string _familyName = null;
        private string _familyCode = null;
        private string _familyId = null;


        public FamilyViewModel()
        {
            
            JoinToFamily = new Command(async () =>
            {
                if (!await DatabaseLogic.IsExistFamilyAsync(ushort.Parse(FamilyId)))
                {
                    await Task.Run(async () =>
                    {
                        await Task.Delay(500);
                        App.AlertSvc.ShowAlert("Ooops", "You family with this Id does not exist");
                        FamilyId = null;
                    });
                }
                else
                {


                    await Task.Run(async () =>
                    {
                        _family = await DatabaseLogic.GetFullFamilyInformationAsync(ushort.Parse(_familyId));
                    });

                }

            },
            () => FamilyId != null);
            CreateFamily = new Command(() =>
            {


            },
             () => FamilyName != null && FamilyCode != null);



        }

        private static bool IsFamilyIdEmpty() => _user.FamilyId == null;
        public bool IsFamilyEmpty => _isFamilyIdEmpty;
        public string FamilyId
        {
            get => _familyId;
            set
            {
                if (_familyId != value)
                {
                    _familyId = value;
                    OnPropertyChanged();
                }
            }
        }
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
            ((Command)JoinToFamily).ChangeCanExecute();
        }
    }
}
