// Program.cs
using Kassasystem.Domain;
using Kassasystem.Services;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Läser produkter från fil...\n");

        var repo = new FileProductRepository("products.txt");
        var products = repo.namespace Kassasystem.Domain
{
    public class Product
    {
        public int Id { get; }
        public string Name { get; }
        public decimal Price { get; }
        public PriceType PriceType { get; }

        public Product(int id, string name, decimal price, PriceType priceType)
        {
            Id = id;
            Name = name;
            Price = price;
            PriceType = priceType;
        }
    }
}
using Kassasystem.Domain;
using System.Globalization;

namespace Kassasystem.Services
{
    public class FileProductRepository : IProductRepository
    {
        private readonly string _filePath;

        public FileProductRepository(string filePath)
        {
            _filePath = filePath;
        }

        public IReadOnlyCollection<Product> GetAll()
        {
            // existing implementation...
        }
    }
}

foreach (var p in products)
{
    Console.WriteLine($"{p.Id} | {p.Name} | {p.Price} kr | {p.PriceType}");
}

Console.WriteLine("\nWeek 1 klar – filhantering och OOP fungerar!");
Console.ReadLine();
    }
}
// Program.cs
using Kassasystem.Services;
using Kassasystem.Domain;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Läser produkter från fil...\n");

        var repo = new FileProductRepository("products.txt");
        var products = repo.GetAll();

        foreach (var p in products)
        {
            Console.WriteLine($"{p.Id} | {p.Name} | {p.Price} kr | {p.PriceType}");
        }

        Console.WriteLine("\nWeek 1 klar – filhantering och OOP fungerar!");
        Console.ReadLine();
    }
}
// Domain/PriceType.cs
namespace Kassasystem.Domain
{
    public enum PriceType
    {
        PerUnit,
        PerKilogram
    }
}
// Domain/Product.cs
namespace Kassasystem.Domain
{
    public class Product
    {
        public int Id { get; }
        public string Name { get; }
        public decimal Price { get; }
        public PriceType PriceType { get; }

        public Product(int id, string name, decimal price, PriceType priceType)
        {
            Id = id;
            Name = name;
            Price = price;
            PriceType = priceType;
        }
    }
}
// Services/IProductRepository.cs
using Kassasystem.Domain;

namespace Kassasystem.Services
{
    public interface IProductRepository
    {
        IReadOnlyCollection<Product> GetAll();
    }
}
// Services/FileProductRepository.cs
using Kassasystem.Domain;
using System.Globalization;

namespace Kassasystem.Services
{
    public class FileProductRepository : IProductRepository
    {
        private readonly string _filePath;

        public FileProductRepository(string filePath)
        {
            _filePath = filePath;
        }

        public IReadOnlyCollection<Product> GetAll()
        {
            if (!File.Exists(_filePath))
                throw new FileNotFoundException("Produktfil saknas.", _filePath);

            var products = new List<Product>();

            foreach (var line in File.ReadAllLines(_filePath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(';');
                if (parts.Length != 4) continue;

                int id = int.Parse(parts[0]);
                string name = parts[1];
                decimal price = decimal.Parse(parts[2], CultureInfo.InvariantCulture);

                PriceType type = parts[3].Equals("Kg", StringComparison.OrdinalIgnoreCase)
                    ? PriceType.PerKilogram
                    : PriceType.PerUnit;

                products.Add(new Product(id, name, price, type));
            }

            return products;
        }
    }
}
