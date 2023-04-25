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

        private DateTime? _dateOfPurshchase { get; set; } = null;

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Expenses> Expenses { get; set; }
        private string _chosenCategoryForAdd { get; set; } 
        private string _chosenCategoryForTable { get; set; }
        public List<string> CategoryNames { get; set; }

        public ICommand ChangeCategory {get; set;}
        

        public ExpensesViewModel()
        {
            CategoryNames = _categoriesRepository.GetAllUsersCategoriesName(User.Id);
            Expenses = new ObservableCollection<Expenses>(_expensesRepositiry.GetUserExpensesByCategoty(User.Id , CategoryNames[0]));

            ChangeCategory = new Command(() => 
            {
                Expenses = new ObservableCollection<Expenses>(_expensesRepositiry.GetUserExpensesByCategoty(User.Id, ChosenCategoryForTable));
            });

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
                if(value != _chosenCategoryForAdd)
                {
                    _chosenCategoryForAdd = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime? DateOfPurshchase
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
