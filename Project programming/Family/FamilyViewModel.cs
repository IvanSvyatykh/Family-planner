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
        private string _familyNameCreation { get; set; } = null;
        private string _familyPasswordJoin { get; set; } = null;
        private string _familyIdJoin { get; set; } = null;
        private string _familyPasswordCreation { get; set; } = null;
        private string _repeatedFamilyPasswordCreation { get; set; } = null;


        public FamilyViewModel()
        {

            JoinToFamily = new Command(async () =>
            {
                if (!ushort.TryParse(UniqueFamilyIdJoin, out _))
                {
                    await Task.Run(async () =>
                    {
                        await Task.Delay(500);
                        App.AlertSvc.ShowAlert("Ooops", "You write string, which can not be an Id");
                        UniqueFamilyIdJoin = null;
                    });
                }
                else if (!await DatabaseLogic.IsExistFamilyAsync(ushort.Parse(UniqueFamilyIdJoin)))
                {
                    await Task.Run(async () =>
                    {
                        await Task.Delay(500);
                        App.AlertSvc.ShowAlert("", "Family with this Id does not exist");
                        UniqueFamilyIdJoin = null;
                    });
                }
                else if (await DatabaseLogic.IsFamilyPasswordCorrectAsync(ushort.Parse(UniqueFamilyIdJoin), ushort.Parse(FamilyPasswordJoin)))
                {
                    if (await DatabaseLogic.AddFamilyIdToUserAsync(ushort.Parse(UniqueFamilyIdJoin), _user.Email)) ;
                    {
                        await Task.Run(async () =>
                        {
                            await Task.Delay(500);
                            App.AlertSvc.ShowAlert("", "Sorry, but something goes wrong and we can not add Family Id");
                        });

                    }
                    await Shell.Current.GoToAsync("FamilyView");

                }
                else
                {
                    await Task.Delay(500);
                    App.AlertSvc.ShowAlert("", "Password isn't correct");
                }
            });

            CreateFamily = new Command(async () =>
            {
                if (!ushort.TryParse(FamilyPasswordCreation, out _) || !ushort.TryParse(RepeatedFamilyPasswordCreation, out _))
                {
                    await Task.Delay(500);
                    App.AlertSvc.ShowAlert("", "Password and repeted password are not equal");
                    RepeatedFamilyPasswordCreation = null;
                    FamilyPasswordCreation = null;
                }
                else if (FamilyNameCreation == null || FamilyNameCreation=="")
                {
                    await Task.Delay(500);
                    App.AlertSvc.ShowAlert("Sorry", "But Name of Family can not be null");
                }
                

            });



        }

        private static bool IsFamilyIdEmpty() => _user.FamilyId == null;
        public string RepeatedFamilyPasswordCreation
        {
            get => _repeatedFamilyPasswordCreation;
            set
            {
                if (_repeatedFamilyPasswordCreation != value)
                {
                    _repeatedFamilyPasswordCreation = value;
                    FamilyCreation();
                }
            }
        }
        public string FamilyPasswordCreation
        {
            get => _familyPasswordCreation;
            set
            {
                if (_familyPasswordCreation != value)
                {
                    _familyPasswordCreation = value;
                    FamilyCreation();
                }
            }
        }
        public bool IsFamilyEmpty => _isFamilyIdEmpty;
        public string UniqueFamilyIdJoin
        {
            get => _familyIdJoin;
            set
            {
                if (_familyIdJoin != value)
                {
                    _familyIdJoin = value;
                    JoiningToFamily();
                }
            }
        }
        public string FamilyNameCreation
        {
            get => _familyNameCreation;
            set
            {
                if (_familyNameCreation != value)
                {
                    _familyNameCreation = value;
                    FamilyCreation();
                }
            }
        }
        public string FamilyPasswordJoin
        {
            get => _familyPasswordJoin;
            set
            {
                if (_familyPasswordJoin != value)
                {
                    _familyPasswordJoin = value;
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
