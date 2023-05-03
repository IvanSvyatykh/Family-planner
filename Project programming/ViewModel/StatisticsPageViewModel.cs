using AppService;
using Classes;
using Database;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace StatisticsPage
{
    public class StatisticsPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<DataForStatistics> Data { get; set; } = new ObservableCollection<DataForStatistics>();
        public ObservableCollection<string> CategoryNames { get; set; }
        public List<string> Monthes { get; set; }
        public ICommand ChangeData {  get; set; }   

        private ExtendedMonth _extendedMonthes = new ExtendedMonth();

        private static Dictionary<string, object> _statisticsPageCurrentData = (App.Current as App).currentData;

        private User _user = _statisticsPageCurrentData["User"] as User;
        private string _chosenMonth { get; set; }

        private SQLGoodsCategoriesRepository _categoriesRepository = new SQLGoodsCategoriesRepository();

        private SQLExpensesRepository _expensesRepository = new SQLExpensesRepository();

        public StatisticsPageViewModel()
        {
            InizalizationFields();
            AddDataForStatistics();

            ChangeData = new Command(() =>
            {
                ChangeDataForStatistics();
            });

        }

        private void ChangeDataForStatistics()
        {
            ObservableCollection<Expenses> ExpensesInMonth = new ObservableCollection<Expenses>(_expensesRepository.GetUserExpensesByMonth(_user.Id,
               _extendedMonthes.GetMonthInByteFromString(ChosenMonth)));

            Data.Clear();

            foreach (var category in CategoryNames)
            {
                DataForStatistics data = new DataForStatistics(_expensesRepository.GetUserExpensesByCategotyAndDate(_user.Id, category,
                    _extendedMonthes.GetMonthInByteFromString(ChosenMonth)));
                Data.Add(data);
            }
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
            ChosenMonth = _extendedMonthes.GetMonthInStringFromByte((byte)DateTime.Now.Month);
            Monthes = _extendedMonthes.GetAllMonthes();

            CategoryNames = new ObservableCollection<string>(_categoriesRepository.GetAllUsersCategoriesName(_user.Id));
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
