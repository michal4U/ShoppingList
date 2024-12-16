public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public decimal Quantity { get; set; } // Ilość
    public string Volume { get; set; } // Jednostka objętości (string)
    public bool IsSelected { get; set; }
}
