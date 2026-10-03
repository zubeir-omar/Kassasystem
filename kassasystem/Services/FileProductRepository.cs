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
                decimal price = decimal.Parse(parts[2], System.Globalization.CultureInfo.InvariantCulture);

                PriceType type = parts[3].Equals("Kg", StringComparison.OrdinalIgnoreCase)
                    ? PriceType.PerKilogram
                    : PriceType.PerUnit;

                products.Add(new Product(id, name, price, type));
            }

            return products;
        }
    }
}
