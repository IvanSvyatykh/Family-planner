using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AccountViewModel
{
    public class AccountPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isAccountPage = false;

        public bool IsAccountPage
        {
            get => _isAccountPage;
            set
            {
                if(Shell.Current.IsBusy)
                {
                    _isAccountPage=true;
                }
            }
        }



    }
}
