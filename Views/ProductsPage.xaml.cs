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
            var products = fileService.LoadProducts(); // Za�aduj produkty z pliku XML
            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(product);
            }
        }

        // Metoda obs�uguj�ca klikni�cie na produkt
        private void OnProductClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var product = button?.BindingContext as Product;

            if (product != null)
            {
                // Zmie� stan "IsSelected" (przekre�lenie/zmiana koloru)
                product.IsSelected = !product.IsSelected;

                // Od�wie� widok, aby pokaza� zmiany
                this.InvalidateMeasure(); // Od�wie�enie ca�ej strony
            }
        }

        // Metoda usuwania produktu
        private void OnDeleteClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var product = button?.BindingContext as Product;
            if (product != null)
            {
                fileService.DeleteProduct(product.Id); // Usu� produkt z pliku XML
                LoadProducts(); // Od�wie� list� po usuni�ciu
            }
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("addproduct"); // Przejd� do strony dodawania produktu
        }
    }
}
