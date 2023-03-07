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
        public ICommand ShowParametersCommand => new Command(ChangeShowParameters);

        private bool _showParametr = true;
        public AccountPageViewModel() 
        {

        }
        public bool ShowParameters
        {
            get => _showParametr; 
            set
            {
                if (value == _showParametr) return;
                _showParametr = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ShowParameters"));
            }
        }
        public void ChangeShowParameters()
        {
            _showParametr = !_showParametr;
        }



    }
}
