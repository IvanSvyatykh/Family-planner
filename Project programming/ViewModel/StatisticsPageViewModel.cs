using AppService;
using Classes;
using Database;
using DataCollector;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WorkWithEmail;

namespace StatisticsPage
{
    public class StatisticsPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<DataForStatistics> Data { get; set; } = new ObservableCollection<DataForStatistics>();
        public ObservableCollection<string> CategoryNames { get; set; }
        public List<string> AccountsEmail { get; set; } = new List<string>();
        public List<string> Monthes { get; set; }
        public ICommand ChangeData { get; set; }

        private ExtendedMonth _extendedMonthes = new ExtendedMonth();

        private IAppData _appData = DependencyService.Get<IAppData>();
        private List<FamilyMember> _familyMembers { get; set; } = new List<FamilyMember>();

        private User _user;

        private Family _family;
        private string _chosenMonth { get; set; }
        private string _chosenMember { get; set; }
        private bool _isCreator { get; set; }

        private SQLGoodsCategoriesRepository _categoriesRepository = new SQLGoodsCategoriesRepository();

        private SQLExpensesRepository _expensesRepository = new SQLExpensesRepository();

        private SQLUserRepository _userRepository = new SQLUserRepository();

        public StatisticsPageViewModel()
        {
            InizalizationFields();
            AddDataForStatistics();

            ChangeData = new Command(async() =>
            {
                if (!CheckEmailCorectness.ConnectionAvailable())
                {
                    await App.AlertSvc.ShowAlertAsync("", "There is no internet, check your connection, please");
                }
                else
                {
                    ChangeDataForStatistics();
                }
                    
            });         
        }

        private void ChangeDataForStatistics()
        {
            
            FamilyMember user = _familyMembers.Where(f => f.MemberEmail.Equals(ChosenMember)).FirstOrDefault();

            uint Id;

            if (user == null) 
            {
                CategoryNames = new ObservableCollection<string>(_categoriesRepository.GetAllUsersCategoriesName(_user.Id));
                Id = _user.Id;
            }
            else
            {
                CategoryNames = new ObservableCollection<string>(_categoriesRepository.GetAllUsersCategoriesName(user.Id));
                Id = user.Id;
            }

            ObservableCollection<Expenses> ExpensesInMonth = new ObservableCollection<Expenses>(_expensesRepository.GetUserExpensesByMonth(_user.Id,
               _extendedMonthes.GetMonthInByteFromString(ChosenMonth)));

            Data.Clear();

            foreach (var category in CategoryNames)
            {
                DataForStatistics data = new DataForStatistics(_expensesRepository.GetUserExpensesByCategotyAndDate(Id, category,
                    _extendedMonthes.GetMonthInByteFromString(ChosenMonth)));
                Data.Add(data);
            }
        }

        private List<FamilyMember> GetAllFamilyAccount(List<User> users)
        {
            List<FamilyMember> members = new List<FamilyMember>();

            if (users != null)
            {
                foreach (var user in users)
                {
                    FamilyMember familyMember = new FamilyMember(user.Name, user.Email, user.FamilyId, user.Id);
                    AccountsEmail.Add(user.Email);
                    members.Add(familyMember);
                }
            }
            return members;

        }

        private void AddDataForStatistics()
        {
            ObservableCollection<Expenses> ExpensesInMonth = new ObservableCollection<Expenses>(_expensesRepository.GetUserExpensesByMonth(_user.Id,
                _extendedMonthes.GetMonthInByteFromString(ChosenMonth)));

            foreach (var category in CategoryNames)
            {
                DataForStatistics data = new DataForStatistics(_expensesRepository.GetUserExpensesByCategotyAndDate(_user.Id, category,
                    _extendedMonthes.GetMonthInByteFromString(ChosenMonth)));
                Data.Add(data);
            }

        }

        private void InizalizationFields()
        {
            _user = _appData.User;
            _family = _appData.Family;
            ChosenMonth = _extendedMonthes.GetMonthInStringFromByte((byte)DateTime.Now.Month);
            Monthes = _extendedMonthes.GetAllMonthes();

            _familyMembers = GetAllFamilyAccount(_userRepository.GetAllAccountWithFamilyId(_user.FamilyId));

            IsCreator = (_family !=null && _user.Email.Equals(_family.Email));

            ChosenMember = _user.Email;

            CategoryNames = new ObservableCollection<string>(_categoriesRepository.GetAllUsersCategoriesName(_user.Id));
        }

        public bool IsCreator
        {
            get => _isCreator;

            set
            {
                if (value != _isCreator)
                {
                    _isCreator = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ChosenMember
        {
            get => _chosenMember;

            set
            {
                if (value != _chosenMember)
                {
                    if (_chosenMember == null)
                    {
                        _chosenMember = _user.Email;
                        OnPropertyChanged();
                    }
                    else
                    {
                        _chosenMember = value;
                        OnPropertyChanged();
                    }
                }
            }
        }

        public string ChosenMonth
        {
            get => _chosenMonth;
            set
            {
                if (value != _chosenMonth)
                {
                    _chosenMonth = value;
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