using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AppService;
using Classes;
using Database;

namespace GoodsCategotyPage
{
    public class GoodsCategoryPageViewModel : INotifyPropertyChanged
    {
        private static Dictionary<string, object> _goodsCategoriesPageCurrentData = (App.Current as App).currentData;

        private User _user = _goodsCategoriesPageCurrentData["User"] as User;

        private string _newCategory = null;

        private SQLGoodsCategoriesRepository _categoriesRepository = new SQLGoodsCategoriesRepository();

        private SQLExpensesRepository _expensesRepository = new SQLExpensesRepository();

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<GoodsCategory> Categories { get; set; }
        public ObservableCollection<GoodsCategory> SelectedCategories { get; set; } = new ObservableCollection<GoodsCategory>();
        public ICommand RemoveCategory { get; set; }
        public ICommand AddCategory { get; set; }


        public GoodsCategoryPageViewModel()
        {

            Categories = new ObservableCollection<GoodsCategory>(_categoriesRepository.GetAllUsersCategories(_user.Id));

            RemoveCategory = new Command(async () =>
            {
                if (SelectedCategories.Count == 0)
                {
                    await App.AlertSvc.ShowAlertAsync("", "You didn't choose any categories");
                }
                else if (await App.AlertSvc.ShowConfirmationAsync("Attention !", "If you delete category of good you will delte all expenses in this category"))
                {
                    foreach (var category in SelectedCategories)
                    {
                        if (!await _categoriesRepository.RemoveCategory(category) & !await _expensesRepository.RevomveAllCategoryExpensesAsync(_user.Id, category.Name))
                        {
                            await App.AlertSvc.ShowAlertAsync("", "Something goes wrong");
                        }
                        Categories.Remove(category);
                    }
                }
            });

            AddCategory = new Command(async () =>
            {
                if (Categories.Count == 0 || !Categories.All(c => c.Name.Equals(NewCategory)))
                {
                    Categories.Add(await _categoriesRepository.AddCategoryAsync(NewCategory, _user.Id));
                    NewCategory = null;
                }
                else
                {
                    await App.AlertSvc.ShowAlertAsync("", "You already have this type of category");
                    NewCategory = null;
                }
            });


        }

        public string NewCategory
        {
            get => _newCategory;
            set
            {
                if (value != _newCategory)
                {
                    _newCategory = value;
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
