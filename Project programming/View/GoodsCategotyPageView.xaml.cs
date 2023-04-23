using UraniumUI.Material.Controls;
namespace GoodsCategotyPage
{
    public partial class GoodsCategotyPageView : ContentPage
    {
        public GoodsCategotyPageView()
        {
            InitializeComponent();
            BindingContext = new GoodsCategoryPageViewModel();  
        }
    }
}

