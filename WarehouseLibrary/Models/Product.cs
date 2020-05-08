using System;

namespace WarehouseLibrary.Models
{
    [Serializable]
    public class Product
    {
        public string Name { get; }
        public string Unit { get; }
        public int Count { get; internal set; }
        public decimal Price { get; internal set; }
        public DateTime ReceiptDate { get; }
        public Supplier Supplier { get; internal set; }
        public Supply Supply { get; internal set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса Product
        /// </summary>
        public Product(string name, string unit, int count, decimal price, Supplier supplier)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "Наименование не может быть null или пустой строкой.");
            }

            if (string.IsNullOrWhiteSpace(unit))
            {
                throw new ArgumentNullException(nameof(name), "Единица измерения не может быть null или пустой строкой.");
            }

            if (count <= 0)
            {
                throw new ArgumentException("Количество не может быть меньше или равно нулю.", nameof(count));
            }

            if (price <= 0)
            {
                throw new ArgumentException("Цена не может быть меньше или равна нулю.", nameof(price));
            }

            Name = name;
            Unit = unit;
            Count = count;
            Price = price;
            Supplier = supplier;
            ReceiptDate = DateTime.Now;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
