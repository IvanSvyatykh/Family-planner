using Microsoft.Maui.Graphics;
using AppService;
using Database;
using System.ComponentModel;
using Classes;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Linq;

namespace StatisticsPage
{
    public class StatisticsPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand DrawNewGrapics { get; set; }
        public ObservableCollection<string> CategoryNames { get; set; }
        public ObservableCollection<string> Monthes { get; set; }
        public ObservableCollection<Expenses> ExpensesInMonth { get; set; }
        public ObservableCollection<DataForGrapics> Data { get; set; } = new ObservableCollection<DataForGrapics>();

        private ExtendedMonth _extendedMonthes = new ExtendedMonth();

        private static Dictionary<string, object> _statisticsPageCurrentData = (App.Current as App).currentData;

        private User _user = _statisticsPageCurrentData["User"] as User;
        private string _chosenCategory { get; set; }
        private string _chosenMonth { get; set; }

        private SQLGoodsCategoriesRepository _categoriesRepository = new SQLGoodsCategoriesRepository();

        private SQLExpensesRepository _expensesRepository = new SQLExpensesRepository();

        public StatisticsPageViewModel()
        {
            ChosenMonth = _extendedMonthes.GetMonthInStringFromByte((byte)DateTime.Now.Month);
            Monthes = new ObservableCollection<string>(_extendedMonthes.GetAllMonthes());

            CategoryNames = new ObservableCollection<string>(_categoriesRepository.GetAllUsersCategoriesName(_user.Id));

            if (CategoryNames.Count() != 0)
            {
                ChosenCategory = CategoryNames[0];
            }

            ExpensesInMonth = new ObservableCollection<Expenses>(_expensesRepository.GetUserExpensesByCategotyAndDate(_user.Id, ChosenCategory, _extendedMonthes.GetMonthInByteFromString(ChosenMonth)));

            byte dayInMonth = (byte)DateTime.DaysInMonth(_extendedMonthes.GetCurrentYear(), _extendedMonthes.GetMonthInByteFromString(ChosenMonth));

            for (byte i = 1; i < dayInMonth; i++)
            {
                Data.Add(new DataForGrapics(i, SumInDay(i)));
            }

            DrawNewGrapics = new Command(async () =>
            {
                await Task.Run(() =>
                {
                    ExpensesInMonth = new ObservableCollection<Expenses>(_expensesRepository.GetUserExpensesByCategotyAndDate(_user.Id, ChosenCategory, _extendedMonthes.GetMonthInByteFromString(ChosenMonth)));
                    Data.Clear();
                    byte dayInMonth = (byte)DateTime.DaysInMonth(_extendedMonthes.GetCurrentYear(), _extendedMonthes.GetMonthInByteFromString(ChosenMonth));

                    for (byte i = 1; i < dayInMonth; i++)
                    {
                        Data.Add(new DataForGrapics(i, SumInDay(i)));
                    }
                });
            });

        }

        private uint SumInDay(byte date)
        {
            uint count = 0;
            foreach(var e in ExpensesInMonth)
            {
                if (e.ExpensesDate.Day == date)
                { 
                    count += e.Cost;
                }
            }

            return count;
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

        public string ChosenCategory
        {
            get => _chosenCategory;

            set
            {
                if (value != _chosenCategory)
                {
                    _chosenCategory = value;
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
