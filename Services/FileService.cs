using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using ShoppingList.Models;

namespace ShoppingList.Services
{
    public class FileService
    {
        ObservableCollection<Product> Products = new ObservableCollection<Product>();
        ObservableCollection<Category> Categories = new ObservableCollection<Category>();
        ObservableCollection<string> Volumes = new ObservableCollection<string>(); // Zmieniono na ObservableCollection<string>

        public ObservableCollection<Product> LoadProducts()
        {
            Products.Clear();
            var filename = "products.xml";
            var fullpath = Path.Combine(FileSystem.AppDataDirectory, filename);

            if (!File.Exists(fullpath))
            {
                GenerateDefaultProducts(); // Jeśli plik nie istnieje, generuj dane domyślne
            }
            else
            {
                var xmlDoc = XDocument.Load(fullpath);
                var products = xmlDoc.Root.Elements("Product")
                    .Select(x => new Product
                    {
                        Id = (int)x.Element("Id"),
                        Name = (string)x.Element("Name"),
                        CategoryId = (int)x.Element("CategoryId"),
                        Quantity = (int?)x.Element("Quantity") ?? 1 // Jeśli nie ma ilości, ustaw na 1
                    });

                Products.Clear();
                foreach (var product in products)
                {
                    Products.Add(product);
                }
            }

            return Products;
        }


        private void GenerateDefaultProducts()
        {
            var filepath = "products.xml";

            var fullpath = Path.Combine(FileSystem.AppDataDirectory, filepath);

            List<Product> DefaultProducts = new List<Product>
            {
                new Product{Id = 1, Name = "Bread", CategoryId = 1, Quantity = 1, Volume = "sztuka"},
                new Product{Id = 2, Name = "Water", CategoryId = 2, Quantity = 1, Volume = "l"},
                new Product{Id = 3, Name = "Wine", CategoryId = 2, Quantity = 1, Volume = "l"}
            };

            var xml = new XDocument(
                new XElement("Products",
                    DefaultProducts.Select(product =>
                        new XElement("Product",
                            new XElement("Id", product.Id),
                            new XElement("Name", product.Name),
                            new XElement("CategoryId", product.CategoryId),
                            new XElement("Quantity", product.Quantity),
                            new XElement("Volume", product.Volume) // Zapisanie jednostki objętości
                        ))
                )
            );

            Products.Clear();
            foreach (var product in DefaultProducts)
            {
                Products.Add(product);
            }

            xml.Save(fullpath);
        }

        public ObservableCollection<Category> LoadCategories()
        {
            var filename = "categories.xml";

            var fullpath = Path.Combine(FileSystem.AppDataDirectory, filename);

            if (!File.Exists(fullpath))
            {
                GenerateDefaultCategories();
            }
            else
            {
                var xmlDoc = XDocument.Load(fullpath);
                var categories = xmlDoc.Root.Elements("Category")
                    .Select(x => new Category
                    {
                        Id = (int)x.Element("Id"),
                        Name = (string)x.Element("Name")
                    });

                Categories.Clear();
                foreach (var category in categories)
                {
                    Categories.Add(category);
                }
            }

            return Categories;
        }

        private void GenerateDefaultCategories()
        {
            var filename = "categories.xml";

            var fullpath = Path.Combine(FileSystem.AppDataDirectory, filename);

            List<Category> DefaultCategories = new List<Category> {
                new Category { Id = 1, Name = "Bread" },
                new Category { Id = 2, Name = "Drinks" },
                new Category { Id = 3, Name = "Vegetables" }
            };

            var xml = new XDocument(
                new XElement("Categories",
                    DefaultCategories.Select(c =>
                        new XElement("Category",
                            new XElement("Id", c.Id),
                            new XElement("Name", c.Name)
                        )
                    )
                )
            );

            Categories.Clear();
            foreach (var category in DefaultCategories)
            {
                Categories.Add(category);
            }

            xml.Save(fullpath);
        }

        public int GetCategoryIdByName(string categoryName)
        {
            if (Categories.Count == 0)
            {
                LoadCategories();
            }

            var category = Categories.FirstOrDefault(c => string.Equals(c.Name, categoryName, StringComparison.OrdinalIgnoreCase));

            return category?.Id ?? -1;
        }

        public string GetCategoryNameById(int categoryId)
        {
            if (Categories.Count == 0)
            {
                LoadCategories();
            }

            var category = Categories.FirstOrDefault(c => c.Id == categoryId);

            return category?.Name ?? "Unknown";
        }

        public ObservableCollection<string> LoadVolumes()
        {
            var filename = "volumes.xml";

            var fullpath = Path.Combine(FileSystem.AppDataDirectory, filename);

            if (!File.Exists(fullpath))
            {
                GenerateDefaultVolumes();
            }
            else
            {
                var xmlDoc = XDocument.Load(fullpath);
                var volumes = xmlDoc.Root.Elements("Volume")
                    .Select(x => (string)x.Element("Name"));

                Volumes.Clear();
                foreach (var volume in volumes)
                {
                    Volumes.Add(volume);
                }
            }

            return Volumes;
        }

        private void GenerateDefaultVolumes()
        {
            var filename = "volumes.xml";

            var fullpath = Path.Combine(FileSystem.AppDataDirectory, filename);

            List<string> DefaultVolumes = new List<string> {
                "psc", "l", "ml", "g", "kg"
            };

            var xml = new XDocument(
                new XElement("Volumes",
                    DefaultVolumes.Select(v =>
                        new XElement("Volume",
                            new XElement("Name", v)
                        )
                    )
                )
            );

            Volumes.Clear();
            foreach (var volume in DefaultVolumes)
            {
                Volumes.Add(volume);
            }

            xml.Save(fullpath);
        }

        public void DeleteProduct(int productId)
        {
            var filename = "products.xml";
            var fullpath = Path.Combine(FileSystem.AppDataDirectory, filename);

            if (!File.Exists(fullpath))
            {
                return; // Brak pliku do usunięcia produktu
            }

            var xmlDoc = XDocument.Load(fullpath);
            var productElement = xmlDoc.Root.Elements("Product")
                .FirstOrDefault(x => (int)x.Element("Id") == productId);

            if (productElement != null)
            {
                productElement.Remove(); // Usuń element z XML
                xmlDoc.Save(fullpath);  // Zapisz zmiany do pliku
            }
        }
    }
}
