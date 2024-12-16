namespace ShoppingList.Views;

public partial class ProductView : ContentView
{
	public ProductView()
	{
		InitializeComponent();
	}
    private async void NavigateToAddProductPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("addproduct");
    }

    private async void NavigateToProductsPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("products");
    }
}