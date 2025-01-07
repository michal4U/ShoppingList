using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShoppingList.Models
{
    public class Product : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _volume;
        private int _quantity;
        private bool _isPurchased;
        private int _categoryId; // Id kategorii
        public Category Category { get; set; } // Obiekt kategorii (opcjonalnie)

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                OnPropertyChanged();
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }

        public bool IsPurchased
        {
            get => _isPurchased;
            set
            {
                _isPurchased = value;
                OnPropertyChanged();
            }
        }

        public int CategoryId
        {
            get => _categoryId;
            set
            {
                _categoryId = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}