using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseLibrary.Data;

namespace WarehouseLibrary.Models
{
    [Serializable]
    public class Warehouse
    {
        public List<Product> Products { get; private set; }
        public List<Supplier> Suppliers { get; private set; }
        public List<Supply> Supplies { get; private set; }

        public Warehouse()
        {
            Products = new List<Product>();
            Suppliers = new List<Supplier>();
            Supplies = new List<Supply>();
        }

        /// <summary>
        /// Добавляет поставку в список поставок
        /// </summary>
        /// <param name="supply"></param>
        public void AddSupply(Supply supply)
        {
            if(supply is null)
                throw new ArgumentNullException(nameof(supply), "Поставка не может быть null.");

            Products.AddRange(supply.Products);
            Suppliers.Add(supply.Supplier);
            Supplies.Add(supply);
            Save();
        }

        /// <summary>
        /// Выводит приходную накладную
        /// </summary>
        /// <param name="supply"></param>
        public void PrintPurchaseInvoice(Supply supply)
        {
            if (supply is null)
                throw new ArgumentNullException(nameof(supply), "Поставка не может быть null.");

            Console.Clear();
            Console.WriteLine($"Приходная накладная от {supply.ReceiptDate.ToShortDateString()}");
            Console.WriteLine($"Поставщик: {supply.Supplier.Name}\n");

            Console.WriteLine($"{"№",5}{"Наименование",15}{"Ед. измер.",15}{"Количество",15}{"Цена",10}");
            for (var i = 0; i < supply.Products.Count; i++)
            {
                Product product = supply.Products[i];
                Console.WriteLine($"{(i + 1),5}{product.Name,15}{product.Unit,15}{product.Count,15}{product.Price,10}");
            }

            Console.WriteLine($"\nВсего товаров: {supply.Products.Count}");
            Console.WriteLine($"Итоговая стоимость: {supply.Products.Sum(p => p.Price)}\n");
        }

        /// <summary>
        /// Выводит список всех товаров на складе
        /// </summary>
        public void PrintAllProducts()
        {
            Console.WriteLine($"{"№",5}{"Наименование",15}{"Ед. измерения",15}{"Количество",15}{"Цена",10}{"Дата получения",20}{"Поставщик",15}{"Номер телефона",20}");
            int counter = 1;
            foreach (Product product in Products)
            {
                Console.WriteLine($"{(counter++),5}{product.Name,15}{product.Unit,15}{product.Count,15}{product.Price,10}" +
                                  $"{product.ReceiptDate.ToShortDateString(),20}{product.Supply.Supplier.Name,15}{product.Supply.Supplier.PhoneNumber,20}");
            }
        }

        /// <summary>
        /// Выводит товар с заданным индексом
        /// </summary>
        /// <param name="number"></param>
        public void PrintProduct(int number)
        {
            if (number < 0 || number > Products.Count)
                throw new ArgumentOutOfRangeException();

            Console.Clear();
            Product product = Products[number];
            Console.WriteLine($"{"Наименование",15}{"Ед. измерения",15}{"Количество",15}{"Цена",10}{"Дата получения",20}{"Поставщик",15}{"Номер телефона",20}");
            Console.WriteLine($"{product.Name,15}{product.Unit,15}{product.Count,15}{product.Price,10}" +
                              $"{product.ReceiptDate.ToShortDateString(),20}{product.Supply.Supplier.Name,15}{product.Supply.Supplier.PhoneNumber,20}");
        }

        /// <summary>
        /// Возвращает заданное количество товара с заданным индексом
        /// </summary>
        /// <param name="number"></param>
        /// <param name="count"></param>
        public Product GetProduct(int number, int count)
        {
            if (number < 0 || number > Products.Count)
                throw new ArgumentOutOfRangeException(nameof(number));

            if (count < 0 || count > Products[number].Count)
                throw new ArgumentOutOfRangeException(nameof(count));

            Product product = Products[number];
            product.Count -= count;

            if (product.Count == 0)
            {
                Products.Remove(product);
            }

            return product;
        }

        /// <summary>
        /// Выводит расходную накладную
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="products"></param>
        public void PrintSalesInvoice(string recipient, List<(Product, int)> products)
        {
            if (string.IsNullOrWhiteSpace(recipient))
                throw new ArgumentNullException(nameof(recipient), "Получатель не может быть null или пустой строкой.");

            if (products is null || products.Count == 0)
                throw new ArgumentNullException(nameof(products), "Список продуктов не может быть пустым или null.");

            Console.Clear();
            Console.WriteLine($"Расходная накладная от {DateTime.Now.ToShortDateString()}");
            Console.WriteLine($"Получатель: {recipient}\n");

            int i = 1;
            Console.WriteLine($"{"№",5}{"Наименование",15}{"Ед. измерения",15}{"Количество",15}{"Цена",10}{"Дата получения",20}{"Поставщик",15}{"Номер телефона",20}");
            foreach ((Product product, int count) in products)
            {
                Console.WriteLine($"{i++,5}{product.Name,15}{product.Unit,15}{count,15}{product.Price * count,10}" +
                                  $"{product.ReceiptDate.ToShortDateString(),20}{product.Supply.Supplier.Name,15}{product.Supply.Supplier.PhoneNumber,20}");
            }

            Console.WriteLine($"\nВсего товаров: {products.Count}");
            Console.WriteLine($"Итоговая стоимость: {products.Sum(p => p.Item1.Price)}\n");
        }

        /// <summary>
        /// Сохраняет данные в файл
        /// </summary>
        public void Save()
        {
            new Dao(this).Save();
        }

        /// <summary>
        /// Загружает данные из файла
        /// </summary>
        public void Load()
        {
            new Dao(this).Load();
        }
    }
}
