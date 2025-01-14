using System;
using System.Collections.ObjectModel;
using ShoppingList.Models;
using ShoppingList.Services;
using Microsoft.Maui.Controls;

namespace ShoppingList.Views
{
    public partial class AddProductPage : ContentPage
    {
        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<string> Volumes { get; set; }
        private FileService fileService = new FileService();

        public AddProductPage()
        {
            InitializeComponent();
            Categories = new ObservableCollection<Category>();
            Volumes = new ObservableCollection<string>();
            LoadCategories();
            LoadVolumes();

            CategoryPicker.ItemsSource = Categories;
            VolumePicker.ItemsSource = Volumes;
        }

        private void LoadCategories()
        {
            var categories = fileService.LoadCategories();
            Categories.Clear();
            foreach (var category in categories)
            {
                Categories.Add(category);
            }
        }

        private void LoadVolumes()
        {
            var volumes = fileService.LoadVolumes();
            Volumes.Clear();
            foreach (var volume in volumes)
            {
                Volumes.Add(volume);
            }
        }

        private void OnAddBtnClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(productNameEntry.Text) ||
                !int.TryParse(productQuantityEntry.Text, out int quantity) ||
                CategoryPicker.SelectedItem == null ||
                VolumePicker.SelectedItem == null)
            {
                DisplayAlert("Błąd", "Proszę wypełnić wszystkie pola.", "OK");
                return;
            }

            var newProduct = new Product
            {
                Name = productNameEntry.Text,
                Quantity = quantity,
                Volume = VolumePicker.SelectedItem.ToString(),
                IsPurchased = false,
                CategoryId = ((Category)CategoryPicker.SelectedItem).Id
            };

            fileService.AddProduct(newProduct);
            MessagingCenter.Send(this, "ProductAdded");
            Navigation.PopAsync();
        }
    }
}
