﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Classes;
using WorkWithDatabase;
using AppService;

namespace FamilyRegistrationPage
{
    public class FamilyRegistrationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ICommand CreateFamily { get; set; }
        public ICommand JoinToFamily { get; set; }
        private string _familyNameCreation { get; set; } = null;
        private string _familyPasswordJoin { get; set; } = null;
        private string _creatorEmaiJoin { get; set; } = null;
        private string _familyPasswordCreation { get; set; } = null;
        private string _repeatedFamilyPasswordCreation { get; set; } = null;

        public FamilyRegistrationViewModel()
        {
            JoinToFamily = new Command(async () =>
            {
                if ((App.Current as App)._user.FamilyId != 0 && (App.Current as App)._user.FamilyId != null)
                {
                    await Task.Run(() =>
                    {
                        App.AlertSvc.ShowAlert("", "You are alredy member of group");
                    });
                }
                else if (!await DatabaseLogic.IsExistFamilyAsync(CreatorEmailJoin))
                {
                    await Task.Run(() =>
                    {
                        App.AlertSvc.ShowAlert("", "Family with this Id does not exist");
                        CreatorEmailJoin = null;
                    });
                }
                else if (await DatabaseLogic.IsFamilyPasswordCorrectAsync(CreatorEmailJoin, FamilyPasswordJoin))
                {
                    if (!await DatabaseLogic.AddFamilyToUserAsync(CreatorEmailJoin, (App.Current as App)._user.Email))
                    {
                        await Task.Run(() =>
                        {
                            App.AlertSvc.ShowAlert("", "Sorry, but something goes wrong and we can not add Family Id");
                        });

                    }
                    else
                    {
                        (App.Current as App)._user.FamilyId = await DatabaseLogic.GetFamilyIdAync(_creatorEmaiJoin);
                        (App.Current as App)._family = new Family(FamilyNameCreation, FamilyPasswordCreation, (App.Current as App)._user.Email);
                        App.AlertSvc.ShowAlert("Great", "You successfully connect to family");
                    }
                }
                else
                {
                    App.AlertSvc.ShowAlert("", "Password isn't correct");
                }
            });

            CreateFamily = new Command(async () =>
            {
                if ((App.Current as App)._user.FamilyId != 0 && (App.Current as App)._user.FamilyId != null)
                {
                    await Task.Run(() =>
                    {
                        App.AlertSvc.ShowAlert("", "You are alredy member of group");
                    });
                }
                else if (!FamilyPasswordCreation.Equals(RepeatedFamilyPasswordCreation))
                {
                    App.AlertSvc.ShowAlert("", "Password and repeted password are not equal");
                    RepeatedFamilyPasswordCreation = null;
                    FamilyPasswordCreation = null;
                }
                else if (FamilyNameCreation == null || FamilyNameCreation == "")
                {
                    App.AlertSvc.ShowAlert("Sorry", "But Name of Family can not be null");
                }
                else
                {
                    (App.Current as App)._family = new Family(FamilyNameCreation, FamilyPasswordCreation, (App.Current as App)._user.Email);
                    if (!await DatabaseLogic.CreateFamilyAsync((App.Current as App)._family, (App.Current as App)._user))
                    {
                        App.AlertSvc.ShowAlert("Sorry", "But we can't create family, something goes wrong");
                    }
                    else
                    {                      
                        await Task.Run(() =>
                        {
                            App.AlertSvc.ShowAlert("Good", "Creation is successful");
                        });
                    }

                }
            });
        }

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

        public string CreatorEmailJoin
        {
            get => _creatorEmaiJoin;
            set
            {
                if (_creatorEmaiJoin != value)
                {
                    _creatorEmaiJoin = value;
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
