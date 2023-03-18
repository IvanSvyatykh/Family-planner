using Project_programming;
using Project_programming.Model.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkWithDatabase;
using Classes;

namespace FamilyPage
{
    public class FamilyViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private static User _user = DatabaseLogic.GetFullPersonInformation((App.Current as App).UserEmail);
        private bool _isExistFamily = IsFamilyIdEmpty();
        public ICommand RefreshPage;
        private bool _isRefreshing = false;


        public FamilyViewModel()
        {
            RefreshPage = new Command(() =>
            {
               
              
            });



        }

        private static bool IsFamilyIdEmpty() => _user.FamilyId == null;
        public bool IsRefreshing => _isRefreshing;
        public bool IsExistFamily => _isExistFamily;
        public bool FamilyExist => !_isExistFamily;
    }
}
