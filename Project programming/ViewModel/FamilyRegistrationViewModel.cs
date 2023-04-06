using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Classes;
using AppService;
using Database;

namespace FamilyRegistrationPage
{
    public class FamilyRegistrationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand CreateFamily { get; set; }
        public ICommand JoinToFamily { get; set; }
        private string _familyNameCreation { get; set; } = null;
        private string _familyPasswordJoin { get; set; } = null;
        private string _creatorEmaiJoin { get; set; } = null;
        private string _familyPasswordCreation { get; set; } = null;
        private string _repeatedFamilyPasswordCreation { get; set; } = null;

        private SQLFamilyRepository _familyRepository = new SQLFamilyRepository();

        private SQLUserRepository _userRepository = new SQLUserRepository();
        public FamilyRegistrationViewModel()
        {

        //    JoinToFamily = new Command(async () =>
        //    {
        //        if (CurrentDataContext.GetUserFamailyId != 0)
        //        {
        //            await Task.Run(() =>
        //            {
        //                App.AlertSvc.ShowAlert("", "You are alredy member of group");
        //            });
        //        }
        //        else if (!await _familyRepository.IsExistFamilyAsync(CreatorEmailJoin))
        //        {
        //            await Task.Run(() =>
        //            {
        //                App.AlertSvc.ShowAlert("", "Family with this Id does not exist");
        //                CreatorEmailJoin = null;
        //            });
        //        }
        //        else if (await _familyRepository.IsFamilyPasswordCorrectAsync(CreatorEmailJoin, FamilyPasswordJoin))
        //        {
        //            if (!await _userRepository.AddFamilyToUserAsync(CreatorEmailJoin, CurrentDataContext.GetUserEmail))
        //            {
        //                await Task.Run(() =>
        //                {
        //                    App.AlertSvc.ShowAlert("", "Sorry, but something goes wrong and we can not add Family Id");
        //                });

        //            }
        //            else
        //            {
        //                CurrentDataContext.AddFamilyIdToUser((ushort)await _familyRepository.GetFamilyIdAync(_creatorEmaiJoin));
        //                CurrentDataContext.AddFamily(new Family(FamilyNameCreation, FamilyPasswordCreation, CurrentDataContext.GetUserEmail));
        //                App.AlertSvc.ShowAlert("Great", "You successfully connect to family");
        //            }
        //        }
        //        else
        //        {
        //            App.AlertSvc.ShowAlert("", "Password isn't correct");
        //        }
        //    });

        //    CreateFamily = new Command(async () =>
        //    {
        //        if (CurrentDataContext.GetUserFamailyId != 0)
        //        {
        //            await Task.Run(() =>
        //            {
        //                App.AlertSvc.ShowAlert("", "You are alredy member of group");
        //            });
        //        }
        //        else if (!FamilyPasswordCreation.Equals(RepeatedFamilyPasswordCreation))
        //        {
        //            App.AlertSvc.ShowAlert("", "Password and repeted password are not equal");
        //            RepeatedFamilyPasswordCreation = null;
        //            FamilyPasswordCreation = null;
        //        }
        //        else if (FamilyNameCreation == null || FamilyNameCreation == "")
        //        {
        //            App.AlertSvc.ShowAlert("Sorry", "But Name of Family can not be null");
        //        }
        //        else
        //        {
        //            CurrentDataContext.AddFamily(new Family(FamilyNameCreation, FamilyPasswordCreation, CurrentDataContext.GetUserEmail));
        //            if (!await _familyRepository.CreateFamilyAsync(CurrentDataContext.GetFamily) && !await _userRepository.AddFamilyToUserAsync(CreatorEmailJoin, CurrentDataContext.GetUserEmail))
        //            {
        //                App.AlertSvc.ShowAlert("Sorry", "But we can't create family, something goes wrong");
        //            }
        //            else
        //            {
        //                await Task.Run(() =>
        //                {
        //                    App.AlertSvc.ShowAlert("Good", "Creation is successful");
        //                });
        //            }

        //        }
        //    });
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
