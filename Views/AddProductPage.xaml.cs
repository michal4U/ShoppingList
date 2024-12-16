using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Linq;
using ShoppingList.Models;
using ShoppingList.Services;

namespace ShoppingList.Views;

public partial class AddProductPage : ContentPage
{
    public ObservableCollection<Product> Products { get; set; }
    public ObservableCollection<Category> Categories { get; set; }
    public ObservableCollection<string> Volumes { get; set; } // Teraz lista stringów dla jednostek objętości
    private FileService fileService = new FileService();

    public AddProductPage()
    {
        InitializeComponent();
        fileService = new();
        Categories = new ObservableCollection<Category>();
        Products = new ObservableCollection<Product>();
        Volumes = new ObservableCollection<string>(); // Inicjalizacja listy stringów
        CategoryPicker.ItemsSource = Categories;
        CategoryPicker.ItemDisplayBinding = new Binding("Name");
        VolumePicker.ItemsSource = Volumes; // Teraz przypisanie listy stringów
    }

    private void OnAddBtnClicked(object sender, EventArgs e)
    {
        string filepath = "products.xml";
        string fullpath = Path.Combine(FileSystem.AppDataDirectory, filepath);
        var products = fileService.LoadProducts();
        var maxId = products.Count;
        string productName = productNameEntry.Text;
        string productQuantityText = productQuantityEntry.Text;

        // Sprawdzenie, czy ilość jest liczbą
        if (!decimal.TryParse(productQuantityText, out decimal productQuantity) || productQuantity <= 0)
        {
            DisplayAlert("Błąd", "Proszę podać prawidłową ilość!", "OK");
            return;
        }

        var selectedCategory = CategoryPicker.SelectedItem as Category;
        if (selectedCategory == null)
        {
            DisplayAlert("Błąd", "Nie wybrano kategorii!", "OK");
            return;
        }

        // Używamy string do przechowywania jednostki objętości
        string selectedVolume = VolumePicker.SelectedItem as string;
        if (string.IsNullOrEmpty(selectedVolume))
        {
            DisplayAlert("Błąd", "Nie wybrano jednostki objętości!", "OK");
            return;
        }

        var newProduct = new Product
        {
            Id = maxId + 1,
            Name = productName,
            CategoryId = selectedCategory.Id,
            Quantity = productQuantity,
            Volume = selectedVolume // Jednostka objętości jako string
        };

        var xmlDoc = XDocument.Load(fullpath);
        xmlDoc.Root.Add(new XElement("Product",
            new XElement("Id", newProduct.Id),
            new XElement("Name", newProduct.Name),
            new XElement("CategoryId", newProduct.CategoryId),
            new XElement("Quantity", newProduct.Quantity),
            new XElement("Volume", newProduct.Volume) // Zapisz jednostkę objętości jako string
        ));
        xmlDoc.Save(fullpath);
        DisplayAlert("Dodano nowy produkt", "Produkt został dodany pomyślnie.", "OK");
        Shell.Current.GoToAsync("products");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        DisplayCategories();
        DisplayVolumes();
    }

    private void DisplayCategories()
    {
        var categories = fileService.LoadCategories();

        Categories.Clear();
        foreach (var category in categories)
        {
            Categories.Add(category);
        }
    }

    private void DisplayVolumes()
    {
        var volumes = fileService.LoadVolumes(); // Zmodyfikuj tę metodę w FileService, jeśli to konieczne

        Volumes.Clear();
        foreach (var volume in volumes)
        {
            Volumes.Add(volume); // Dodanie jednostek objętości do listy
        }
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
