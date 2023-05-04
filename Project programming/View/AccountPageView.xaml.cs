using Classes;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;


namespace AccountPage
{
    public partial class AccountPageView : ContentPage
    {       
        public AccountPageView()
        {
            InitializeComponent();
            
            BindingContext = new AccountPageViewModel();
        }
      

    }
}


