using ShoppingList.Models;
using ShoppingList.Services;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;

namespace ShoppingList.Views
{
    public partial class ProductsPage : ContentPage
    {
        private FileService _fileService; // U¿yj _fileService z wielk¹ liter¹ "S"
        public ObservableCollection<Product> Products { get; private set; }

        public ProductsPage()
        {
            InitializeComponent();
            _fileService = new FileService(); // U¿yj _fileService
            Products = _fileService.LoadProducts();
            BindingContext = this;

            // Subskrybuj wiadomoœæ o dodaniu produktu
            MessagingCenter.Subscribe<AddProductPage>(this, "ProductAdded", (sender) =>
            {
                Products = _fileService.LoadProducts();
                OnPropertyChanged(nameof(Products)); // Powiadom o zmianie
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
                _fileService.DeleteProduct(product.Id); // Usuñ produkt z pliku
                Products.Remove(product); // Usuñ produkt z kolekcji
                
            }
        }

        private void OnProductTapped(object sender, EventArgs e)
        {
            if (sender is Grid grid && grid.BindingContext is Product product)
            {
                
                product.IsPurchased = !product.IsPurchased;
                _fileService.SaveProducts(); // U¿yj _fileService
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