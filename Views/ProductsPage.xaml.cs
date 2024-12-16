using System.Collections.ObjectModel;
using ShoppingList.Models;
using ShoppingList.Services;

namespace ShoppingList.Views
{
    public partial class ProductsPage : ContentPage
    {
        public ObservableCollection<Product> Products { get; set; }
        private FileService fileService;

        public ProductsPage()
        {
            InitializeComponent();
            fileService = new FileService();
            Products = new ObservableCollection<Product>();
            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadProducts();
        }

        private void LoadProducts()
        {
            var products = fileService.LoadProducts(); // Za³aduj produkty z pliku XML
            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(product);
            }
        }

        // Metoda obs³uguj¹ca klikniêcie na produkt
        private void OnProductClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var product = button?.BindingContext as Product;

            if (product != null)
            {
                // Zmieñ stan "IsSelected" (przekreœlenie/zmiana koloru)
                product.IsSelected = !product.IsSelected;

                // Odœwie¿ widok, aby pokazaæ zmiany
                this.InvalidateMeasure(); // Odœwie¿enie ca³ej strony
            }
        }

        // Metoda usuwania produktu
        private void OnDeleteClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var product = button?.BindingContext as Product;
            if (product != null)
            {
                fileService.DeleteProduct(product.Id); // Usuñ produkt z pliku XML
                LoadProducts(); // Odœwie¿ listê po usuniêciu
            }
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("addproduct"); // PrzejdŸ do strony dodawania produktu
        }
    }
}
