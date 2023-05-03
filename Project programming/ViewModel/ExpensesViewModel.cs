using System.ComponentModel;
using Classes;
using Database;
using AppService;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ExpensesPage
{
    public class ExpensesViewModel : INotifyPropertyChanged
    {
        private static Dictionary<string, object> _expensesPageCurrentData = (App.Current as App).currentData;

        private User _user = _expensesPageCurrentData["User"] as User;

        private SQLExpensesRepository _expensesRepositiry = new SQLExpensesRepository();

        private SQLGoodsCategoriesRepository _categoriesRepository = new SQLGoodsCategoriesRepository();

        private uint _cost;

        private ExtendedMonth _extendedMonthes = new ExtendedMonth();
        private string _chosenCategoryForAdd { get; set; }
        private string _chosenCategoryForTable { get; set; }
        private string _chosenMonth { get; set; }

        private DateTime _dateOfPurshchase { get; set; } = DateTime.Today;

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Expenses> Expenses { get; set; }
        public ObservableCollection<string> Monthes { get; set; }
        public ObservableCollection<Expenses> Selected { get; set; } = new ObservableCollection<Expenses>();
        public ObservableCollection<string> CategoryNames { get; set; }
        public ICommand ChangeCategory { get; set; }
        public ICommand AddExpenses { get; set; }
        public ICommand DeleteExpenses { get; set; }
        public ICommand RefreshCategory { get; set; }

        public ExpensesViewModel()
        {
            ChosenMonth = _extendedMonthes.GetMonthInStringFromByte((byte)DateTime.Now.Month);
            Monthes = new ObservableCollection<string>(_extendedMonthes.GetAllMonthes());

            CategoryNames = new ObservableCollection<string>(_categoriesRepository.GetAllUsersCategoriesName(_user.Id));

            if (CategoryNames.Count() != 0)
            {
                ChosenCategoryForTable = CategoryNames[0];
            }

            Expenses = new ObservableCollection<Expenses>(_expensesRepositiry.GetUserExpensesByCategotyAndDate(_user.Id, ChosenCategoryForTable,
                _extendedMonthes.GetMonthInByteFromString(ChosenMonth)));

            ChangeCategory = new Command(() =>
            {
                var ExpensesFromTable = new ObservableCollection<Expenses>(_expensesRepositiry.GetUserExpensesByCategotyAndDate(_user.Id, ChosenCategoryForTable,
                    _extendedMonthes.GetMonthInByteFromString(ChosenMonth)));
                ExpensesFromTable.ToList().Sort((l, r) => l.ExpensesDate.CompareTo(r.ExpensesDate));
                Expenses.Clear();
                Selected.Clear();
                foreach (var category in ExpensesFromTable)
                {

                    Expenses.Add(category);
                    OnPropertyChanged();
                }

                OnPropertyChanged();
            });

            AddExpenses = new Command(async () =>
            {
                if (!await _expensesRepositiry.AddExpenseAsync(new Expenses() { Cost = Cost, UserId = _user.Id, ExpensesName = ChosenCategoryForAdd, ExpensesDate = DateOnly.FromDateTime(DateOfPurshchase) }))
                {
                    await App.AlertSvc.ShowAlertAsync("Sorry", "Something goes wrong we can't add expense");
                }
                ChosenCategoryForAdd = null;
                DateOfPurshchase = DateTime.Today;
                Cost = 0;
            });

            DeleteExpenses = new Command(async () =>
            {
                foreach (var select in Selected)
                {
                    Expenses.Remove(select);
                    if (!await _expensesRepositiry.RemoveExpenseAsync(select))
                    {
                        await App.AlertSvc.ShowAlertAsync("Sorry", "Something goes wrong we can't delete expense");
                    }
                }
                if (Expenses.Count == 0)
                {
                    Expenses.Add(new Expenses() { Cost = 0, ExpensesName = null, UserId = null, Id = 0 });
                }
            });

            RefreshCategory = new Command(() =>
            {
                ObservableCollection<string> NewCategories = new ObservableCollection<string>(_categoriesRepository.GetAllUsersCategoriesName(_user.Id));
                CategoryNames.Clear();

                foreach (var category in NewCategories)
                {
                    CategoryNames.Add(category);
                }
                if (CategoryNames.Count() != 0)
                {
                    ChosenCategoryForTable = CategoryNames[0];
                }
            });
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

        public string ChosenCategoryForTable
        {
            get => _chosenCategoryForTable;

            set
            {
                if (value != _chosenCategoryForTable)
                {
                    _chosenCategoryForTable = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ChosenCategoryForAdd
        {
            get => _chosenCategoryForAdd;

            set
            {
                if (value != _chosenCategoryForAdd)
                {
                    _chosenCategoryForAdd = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime DateOfPurshchase
        {
            get => _dateOfPurshchase;

            set
            {
                if (value != _dateOfPurshchase)
                {
                    _dateOfPurshchase = value;
                }
            }
        }
        public uint Cost
        {
            get => _cost;
            set
            {
                if (value != _cost)
                {
                    _cost = value;
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