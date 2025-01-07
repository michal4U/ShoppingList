using ShoppingList.Models;
using System;
using Microsoft.Maui.Controls;
using ShoppingList.Services;

namespace ShoppingList.Views
{
    public partial class ProductItemView : ContentView
    {
        private FileService _fileService;

        public ProductItemView()
        {
            InitializeComponent();
            _fileService = new FileService(); // Inicjalizacja FileService
        }

        private void OnPurchasedClicked(object sender, EventArgs e)
        {
            if (BindingContext is Product product)
            {
                product.IsPurchased = !product.IsPurchased;
                _fileService.SaveProducts(); // Zapisz zmiany po zaznaczeniu
            }
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            if (BindingContext is Product product)
            {
                DeleteRequested?.Invoke(this, product); // Wywo³aj zdarzenie usuwania
            }
        }


        private void OnIncreaseQuantityClicked(object sender, EventArgs e)
        {
            if (BindingContext is Product product)
            {
                product.Quantity++;
                _fileService.SaveProducts(); // Zapisz zmiany po zwiêkszeniu iloœci
            }
        }

        private void OnDecreaseQuantityClicked(object sender, EventArgs e)
        {
            if (BindingContext is Product product && product.Quantity > 0)
            {
                product.Quantity--;
                _fileService.SaveProducts(); // Zapisz zmiany po zmniejszeniu iloœci
            }
        }

        public event EventHandler<Product> DeleteRequested;
    }
}