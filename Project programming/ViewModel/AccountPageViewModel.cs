﻿using Project_programming;
using System.ComponentModel;
using WorkWithDatabase;
using Classes;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AppService;

namespace AccountPage
{
    public class AccountPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static User _user = DatabaseLogic.GetFullPersonInformation((App.Current as App).UserEmail);

        private  Family _family = DatabaseLogic.GetFullFamilyInformation(_user.FamilyId);

        public ICommand SaveChangesInSalary { get; set; }

        public AccountPageViewModel()
        {
            SaveChangesInSalary = new Command(async () =>
            {
                if (uint.TryParse(Salary, out _))
                {
                    await DatabaseLogic.AddSalaryToUser(Email, uint.Parse(Salary));
                    await Task.Delay(500);
                    await Task.Run(() =>
                    {
                        App.AlertSvc.ShowAlert("", "Salaru succefully changed", "");
                    });
                }
            });


        }

        public string Name => "Welcome, " + _user.Name;
        public string Email => _user.Email;
        public string FamilyName
        {
            get
            {
                if (_family == null)
                {
                    return null;
                }
                else
                {
                    return "Family Name : " + _family.Name;
                }


            }
        }
        public string Salary
        {
            get => _user.Salary.ToString();
            set
            {
                if (value != _user.Salary.ToString() && uint.TryParse(value, out _))
                {
                    _user.Salary = uint.Parse(value);
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
