using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Project_programming.WorkWithEmail;
using System.Runtime.CompilerServices;

namespace Project_programming
{
    public class MainPageViewModel : INotifyPropertyChanged
    {       
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand RegistarationPageCommand { get; set; }
        public ICommand ForgottenPasswordPage { get; set; }
        public MainPageViewModel()
        {
            RegistarationPageCommand = new Command(() =>
            {
              Shell.Current.GoToAsync("RegistrationPage");
            });
            ForgottenPasswordPage = new Command(() =>
            {
                Shell.Current.GoToAsync("ForgottenPasswordPage");
            });          
        }      
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }



}
    




