using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using ShoppingList.Models;
using Microsoft.Maui.Storage; // Upewnij się, że masz odpowiednią przestrzeń nazw

namespace ShoppingList.Services
{
    public class FileService
    {
        private const string ProductsFileName = "produkty.json";
        private const string CategoriesFileName = "kategorie.json";
        private const string VolumesFileName = "jednostki.json";
        private readonly string _productsPath;
        private readonly string _categoriesPath;
        private readonly string _volumesPath;

        public ObservableCollection<Product> Products { get; private set; }

        public FileService()
        {
            _productsPath = Path.Combine(FileSystem.AppDataDirectory, ProductsFileName);
            _categoriesPath = Path.Combine(FileSystem.AppDataDirectory, CategoriesFileName);
            _volumesPath = Path.Combine(FileSystem.AppDataDirectory, VolumesFileName);
            Products = new ObservableCollection<Product>();
            LoadProducts(); // Ładuj produkty przy inicjalizacji
        }

        public ObservableCollection<Product> LoadProducts()
        {
            if (!File.Exists(_productsPath))
            {
                GenerateDefaultProducts();
                return Products;
            }

            var json = File.ReadAllText(_productsPath);
            var products = JsonSerializer.Deserialize<List<Product>>(json);
            Products.Clear();
            if (products != null)
            {
                foreach (var product in products)
                {
                    Products.Add(product);
                }
            }

            return Products;
        }

        private void GenerateDefaultProducts()
        {
            var defaultProducts = new List<Product>
            {
                new() { Id = 1, Name = "Chleb", Volume = "szt.", Quantity = 1 },
                new() { Id = 2, Name = "Mleko", Volume = "l", Quantity = 1 }
            };

            Products.Clear();
            foreach (var product in defaultProducts)
            {
                Products.Add(product);
            }
            SaveProducts();
        }

        public void SaveProducts()
        {
            var json = JsonSerializer.Serialize(Products);
            File.WriteAllText(_productsPath, json);
        }

        public void AddProduct(Product product)
        {
            Products.Add(product);
            SaveProducts(); // Zapisz po dodaniu produktu
        }

        public void DeleteProduct(int productId)
        {
            var productToRemove = Products.FirstOrDefault(p => p.Id == productId);
            if (productToRemove != null)
            {
                Products.Remove(productToRemove);
                SaveProducts(); // Zapisz po usunięciu produktu
            }
            SaveProducts() ;
        }

        public ObservableCollection<Category> LoadCategories()
        {
            var categories = new ObservableCollection<Category>();

            if (!File.Exists(_categoriesPath))
            {
                // Dodaj domyślne kategorie, jeśli plik nie istnieje
                categories.Add(new Category { Id = 1, Name = "Artykuły spożywcze" });
                categories.Add(new Category { Id = 2, Name = "Napoje" });
                return categories;
            }

            var json = File.ReadAllText(_categoriesPath);
            var loadedCategories = JsonSerializer.Deserialize<List<Category>>(json);
            if (loadedCategories != null)
            {
                foreach (var category in loadedCategories)
                {
                    categories.Add(category);
                }
            }

            return categories;
        }

        public ObservableCollection<string> LoadVolumes()
        {
            var volumes = new ObservableCollection<string>();

            if (!File.Exists(_volumesPath))
            {
                // Dodaj domyślne jednostki, jeśli plik nie istnieje
                volumes.Add("szt.");
                volumes.Add("l");
                volumes.Add("kg");
                return volumes;
            }

            var json = File.ReadAllText(_volumesPath);
            var loadedVolumes = JsonSerializer.Deserialize<List<string>>(json);
            if (loadedVolumes != null)
            {
                foreach (var volume in loadedVolumes)
                {
                    volumes.Add(volume);
                }
            }

            return volumes;
        }
    }
}