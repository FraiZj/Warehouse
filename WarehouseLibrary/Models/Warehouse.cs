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

        /// <summary>
        /// Инициализирует новый экземпляр класса Warehouse
        /// </summary>
        public Warehouse()
        {
            Products = new List<Product>();
            Suppliers = new List<Supplier>();
            Supplies = new List<Supply>();
        }

        /// <summary>
        /// Проверяет есть ли на складе дубликат товара и возвращает его
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public Product DuplicateProduct(Product product)
        {
            return Products.SingleOrDefault(p => p.Name == product.Name
                                                 && p.Price == product.Price
                                                 && p.Supplier.Name == product.Supplier.Name);
        }

        /// <summary>
        /// Добавляет поставку в список поставок
        /// </summary>
        /// <param name="supply"></param>
        public void AddSupply(Supply supply)
        {
            if (supply is null)
            {
                throw new ArgumentNullException(nameof(supply), "Поставка не может быть null.");
            }

            Products.AddRange(supply.Products);
            Suppliers.Add(supply.Supplier);
            Supplies.Add(supply);
        }

        /// <summary>
        /// Выводит приходную накладную
        /// </summary>
        /// <param name="supply"></param>
        public void PrintPurchaseInvoice(Supply supply)
        {
            if (supply is null)
            {
                throw new ArgumentNullException(nameof(supply), "Поставка не может быть null.");
            }

            decimal totalCost = 0;

            Console.Clear();
            Console.WriteLine($"Приходная накладная от {supply.ReceiptDate.ToShortDateString()}");
            Console.WriteLine($"Поставщик: {supply.Supplier.Name}\n");
            Console.WriteLine($"{"№",5}{"Наименование",20}{"Ед. измер.",15}{"Количество",15}{"Цена",15}");

            for (var i = 0; i < supply.Products.Count; i++)
            {
                Product product = supply.Products[i];
                Console.WriteLine($"{(i + 1),5}{product.Name,20}{product.Unit,15}{product.Count,15}{product.Price,15}");

                totalCost += product.Price * product.Count;
            }

            Console.WriteLine($"\nВсего товаров: {supply.Products.Count}");
            Console.WriteLine($"Итоговая стоимость: {totalCost}\n");
        }

        /// <summary>
        /// Выводит список всех товаров на складе
        /// </summary>
        public void PrintAllProducts()
        {
            int counter = 1;

            Console.WriteLine($"{"№",5}{"Наименование",20}{"Ед. измерения",15}{"Количество",15}" +
                              $"{"Цена",15}{"Дата получения",20}{"Поставщик",20}{"Номер телефона",20}");

            foreach (Product product in Products)
            {

                Console.WriteLine($"{(counter++),5}{product.Name,20}{product.Unit,15}{product.Count,15}" +
                                  $"{product.Price,15}{product.ReceiptDate.ToShortDateString(),20}" +
                                  $"{product.Supply.Supplier.Name,20}{product.Supply.Supplier.PhoneNumber,20}");
            }
        }

        /// <summary>
        /// Добавляет заданное количество товара
        /// </summary>
        /// <param name="product"></param>
        /// <param name="count"></param>
        public void AddSomeCountOfProduct(Product product, int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            product.Count += count;
        }

        /// <summary>
        /// Удаляет заданное количество товара
        /// </summary>
        /// <param name="product"></param>
        /// <param name="count"></param>
        public void RemoveSomeCountOfProduct(Product product, int count)
        {
            if (count < 0 || count > product.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            product.Count -= count;

            if (product.Count == 0)
            {
                Products.Remove(product);
            }
        }

        /// <summary>
        /// Выводит расходную накладную
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="products"></param>
        public void PrintSalesInvoice(string recipient, List<(Product, int)> products)
        {
            if (string.IsNullOrWhiteSpace(recipient))
            {
                throw new ArgumentNullException(nameof(recipient), "Получатель не может быть null или пустой строкой.");
            }

            if (products is null || products.Count == 0)
            {
                throw new ArgumentNullException(nameof(products), "Список продуктов не может быть пустым или null.");
            }

            int i = 1;
            decimal totalCost = 0;

            Console.Clear();
            Console.WriteLine($"Расходная накладная от {DateTime.Now.ToShortDateString()}");
            Console.WriteLine($"Получатель: {recipient}\n");

            Console.WriteLine($"{"№",5}{"Наименование",20}{"Ед. измерения",15}{"Количество",15}" +
                              $"{"Цена",15}{"Дата получения",20}{"Поставщик",20}{"Номер телефона",20}");

            foreach ((Product product, int count) in products)
            {

                Console.WriteLine($"{i++,5}{product.Name,20}{product.Unit,15}{count,15}" +
                                  $"{product.Price,15}{product.ReceiptDate.ToShortDateString(),20}" +
                                  $"{product.Supply.Supplier.Name,20}{product.Supply.Supplier.PhoneNumber,20}");

                totalCost += product.Price * count;
            }

            Console.WriteLine($"\nВсего товаров: {products.Count}");
            Console.WriteLine($"Итоговая стоимость: {totalCost}\n");
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
