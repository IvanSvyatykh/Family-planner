using Classes;
using Project_programming;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkWithDatabase;

namespace AccountViewModel
{
    public class AccountPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private User _user = DatabaseLogic.GetFullPersinInformation((App.Current as App).UserEmail);
        
        public string Name 
        {
            get =>"Welcome, " + _user.Name;
        }    





    }
}
