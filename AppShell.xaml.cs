using ShoppingList.Views;

namespace ShoppingList
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("list", typeof(ListPage));
            Routing.RegisterRoute("products", typeof(ProductsPage));
            Routing.RegisterRoute("addproduct", typeof(AddProductPage));
        }
    }

}
