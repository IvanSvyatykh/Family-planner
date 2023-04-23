﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppService;
using Classes;
using Database;

namespace GoodsCategotyPage
{
    public class GoodsCategoryPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private SQLGoodsCategoriesRepository _categoriesRepository = new SQLGoodsCategoriesRepository();
        public ObservableCollection<GoodsCategory> Categories { get; set; }
        public ObservableCollection<GoodsCategory> SelectedCategories { get; set; } = new ObservableCollection<GoodsCategory>();

        private static Dictionary<string, object> GoodsCategoriesPageCurrentData = (App.Current as App).currentData;

        private User User = GoodsCategoriesPageCurrentData["User"] as User;

        private string _newCategory = null;
        public ICommand RemoveCategory { get; set; }
        public ICommand AddCategory { get; set; }

        public GoodsCategoryPageViewModel()
        {

            Categories = new ObservableCollection<GoodsCategory>(_categoriesRepository.GetAllUsersCategories(User.Id));

            AddCategory = new Command(async () =>
            {
                if (!Categories.All(c => c.Name.Equals(NewCategory)))
                {
                    Categories.Add(await _categoriesRepository.AddCategoryAsync(NewCategory, User.Id));
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