using System;
using Microsoft.Maui.Controls;

namespace ShoppingList.Views
{
    public partial class ListPage : ContentPage
    {
        public ListPage()
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
}
