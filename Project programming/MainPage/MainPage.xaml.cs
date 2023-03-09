using System.Xml;
using Project_programming.WorkWithEmail;
namespace Project_programming;
public partial class MainPage : ContentPage
{ 
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainPageViewModel();
    }
}