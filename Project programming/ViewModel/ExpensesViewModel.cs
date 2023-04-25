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

namespace ExpensesPage
{
    public class ExpensesViewModel : INotifyPropertyChanged
    {
        private static Dictionary<string, object> ExpensesPageCurrentData = (App.Current as App).currentData;

        private User User = ExpensesPageCurrentData["User"] as User;

        private SQLExpensesRepository _expensesRepositiry = new SQLExpensesRepository();

        private SQLGoodsCategoriesRepository _categoriesRepository = new SQLGoodsCategoriesRepository();

        private uint? _cost;

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Expenses> Expenses { get; set; }
        public GoodsCategory ChosenCategory { get; set; } = new GoodsCategory();
        public List<string> CategoryNames { get; set; }

        private DateTime? _dateOfPurshchase { get; set; } = null;



        public ExpensesViewModel()
        {
            CategoryNames = _categoriesRepository.GetAllUsersCategoriesName(User.Id);

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
