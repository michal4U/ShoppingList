using ShoppingList.Models;
using ShoppingList.Services;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;

namespace ShoppingList.Views
{
    public partial class ProductsPage : ContentPage
    {
        private FileService _fileService; 
        public ObservableCollection<Product> Products { get; private set; }

        public ProductsPage()
        {
            InitializeComponent();
            _fileService = new FileService();
            Products = _fileService.LoadProducts();
            BindingContext = this;

            MessagingCenter.Subscribe<AddProductPage>(this, "ProductAdded", (sender) =>
            {
                Products = _fileService.LoadProducts();
                OnPropertyChanged(nameof(Products));
            });
        }

        private void OnAddClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddProductPage());
        }

        private void OnDeleteClicked(object sender, Product product)
        {
            if (product != null)
            {
                _fileService.DeleteProduct(product.Id);
                Products.Remove(product);
                
            }
        }

        private void OnProductTapped(object sender, EventArgs e)
        {
            if (sender is Grid grid && grid.BindingContext is Product product)
            {
                
                product.IsPurchased = !product.IsPurchased;
                _fileService.SaveProducts();
                SortProducts();
            }
            _fileService.SaveProducts();
        }

        private void SortProducts()
        {
            var sortedProducts = Products
                .OrderBy(p => p.IsPurchased)
                .ThenBy(p => p.Name)
                .ToList();

            Products.Clear();
            foreach (var product in sortedProducts)
            {
                Products.Add(product);
            }
        }
    }
}
