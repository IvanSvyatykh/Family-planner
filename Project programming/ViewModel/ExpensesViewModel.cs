using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Classes;
using Database;
using System.Threading.Tasks;
using AppService;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ExpensesPage
{
    public class ExpensesViewModel : INotifyPropertyChanged
    {
        private static Dictionary<string, object> ExpensesPageCurrentData = (App.Current as App).currentData;

        private User User = ExpensesPageCurrentData["User"] as User;

        private SQLExpensesRepository _expensesRepositiry = new SQLExpensesRepository();

        private SQLGoodsCategoriesRepository _categoriesRepository = new SQLGoodsCategoriesRepository();

        private uint? _cost;

        private ExtendedMonth ExtendedMonthes = new ExtendedMonth();

        public event PropertyChangedEventHandler PropertyChanged;
        private DateTime _dateOfPurshchase { get; set; } = DateTime.Today;
        public ObservableCollection<Expenses> Expenses { get; set; }
        public ObservableCollection<string> Monthes { get; set; }
        public ObservableCollection<Expenses> Selected { get; set; } = new ObservableCollection<Expenses>();
        private string _chosenCategoryForAdd { get; set; }
        private string _chosenCategoryForTable { get; set; }
        private string _chosenMonth { get; set; }
        public ObservableCollection<string> CategoryNames { get; set; }
        public ICommand ChangeCategory { get; set; }
        public ICommand AddExpenses { get; set; }
        public ICommand DeleteExpenses { get; set; }
        public ICommand RefreshCategory { get; set; }

        public ExpensesViewModel()
        {
            ChosenMonth = ExtendedMonthes.GetMonthInStringFromByte((byte)DateTime.Now.Month);
            Monthes = new ObservableCollection<string>(ExtendedMonthes.GetAllMonthes());

            CategoryNames = new ObservableCollection<string>(_categoriesRepository.GetAllUsersCategoriesName(User.Id));

            ChosenCategoryForTable = CategoryNames[0];
            Expenses = new ObservableCollection<Expenses>(_expensesRepositiry.GetUserExpensesByCategoty(User.Id, ChosenCategoryForTable,
                ExtendedMonthes.GetMonthInByteFromString(ChosenMonth)));

            ChangeCategory = new Command(() =>
            {
                var ExpensesFromTable = new ObservableCollection<Expenses>(_expensesRepositiry.GetUserExpensesByCategoty(User.Id, ChosenCategoryForTable,
                    ExtendedMonthes.GetMonthInByteFromString(ChosenMonth)));
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
                if (!await _expensesRepositiry.AddExpenseAsync(new Expenses() { Cost = Cost, UserId = User.Id, ExpensesName = ChosenCategoryForAdd, ExpensesDate = DateOnly.FromDateTime(DateOfPurshchase) }))
                {
                    await App.AlertSvc.ShowAlertAsync("Sorry", "Something goes wrong we can't add expense");
                }
                ChosenCategoryForAdd = null;
                DateOfPurshchase = DateTime.Today;
                Cost = null;
            });

            DeleteExpenses = new Command(async () =>
            {
                foreach (var select in Selected)
                {
                    Expenses.Remove(select);
                    if (!await _expensesRepositiry.RemoveExpense(select))
                    {
                        await App.AlertSvc.ShowAlertAsync("Sorry", "Something goes wrong we can't delete expense");
                    }
                }
                if (Expenses.Count == 0)
                {
                    Expenses.Add(new Expenses() { Cost = null, ExpensesName = null, UserId = null, Id = 0 });
                }
            });

            RefreshCategory = new Command(() =>
            {
                ObservableCollection<string> NewCategories = new ObservableCollection<string>(_categoriesRepository.GetAllUsersCategoriesName(User.Id));
                CategoryNames.Clear();

                foreach (var category in NewCategories) 
                {
                    CategoryNames.Add(category);
                }
                ChosenCategoryForTable = CategoryNames[0];
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
        public uint? Cost
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