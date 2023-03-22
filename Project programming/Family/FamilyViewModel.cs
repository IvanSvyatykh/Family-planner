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
        public event PropertyChangedEventHandler? PropertyChanged;
        private static User _user { get; set; } = DatabaseLogic.GetFullPersonInformation((App.Current as App).UserEmail);
        private static Family _family { get; set; } = null;
        private bool _isFamilyIdEmpty { get; set; } = IsFamilyIdEmpty();
        public ICommand CreateFamily { get; set; }
        public ICommand JoinToFamily { get; set; }
        private string _familyName { get; set; } = null;
        private string _familyCode { get; set; } = null;

        private string _familyId = null;


        public FamilyViewModel()
        {

            JoinToFamily = new Command(async () =>
            {
                if (!ushort.TryParse(UniqueFamilyId, out _))
                {
                    await Task.Run(async () =>
                    {
                        await Task.Delay(500);
                        App.AlertSvc.ShowAlert("Ooops", "You write string, which can not be an Id");
                        UniqueFamilyId = null;
                    });
                }
                else if (!await DatabaseLogic.IsExistFamilyAsync(ushort.Parse(UniqueFamilyId)))
                {
                    await Task.Run(async () =>
                    {
                        await Task.Delay(500);
                        App.AlertSvc.ShowAlert("", "Family with this Id does not exist");
                        UniqueFamilyId = null;
                    });
                }
                else
                {
                    if (!await DatabaseLogic.AddFamilyIdToUser(ushort.Parse(UniqueFamilyId), _user.Email)) ;
                    {
                        await Task.Run(async () =>
                        {
                            await Task.Delay(500);
                            App.AlertSvc.ShowAlert("", "Sorry, but something goes wrong and we can not add Family Id");
                        });

                    }                   
                }



            });

            CreateFamily = new Command(() =>
            {


            });



        }

        private static bool IsFamilyIdEmpty() => _user.FamilyId == null;
        public bool IsFamilyEmpty => _isFamilyIdEmpty;
        public string UniqueFamilyId
        {
            get => _familyId;
            set
            {
                if (_familyId != value)
                {
                    _familyId = value;
                    JoiningToFamily();
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
                    FamilyCreation();
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
