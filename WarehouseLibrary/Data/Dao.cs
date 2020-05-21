using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using WarehouseLibrary.Models;

namespace WarehouseLibrary.Data
{
    class Dao
    {
        private const string FilePath = "warehouse.bin";
        private const string PurchaseInvoicesDirectoryPath = "purchaseInvoices";
        private const string SalesInvoicesDirectoryPath = "salesInvoices";

        private readonly Warehouse _warehouse;

        internal Dao(Warehouse warehouse)
        {
            _warehouse = warehouse;
        }

        /// <summary>
        /// Сохраняет данные в двоичный файл
        /// </summary>
        internal void Save()
        {
            using (var fs = new FileStream(FilePath, FileMode.Create))
            {
                var serializer = new BinaryFormatter();
                serializer.Serialize(fs, _warehouse);
            }
        }

        /// <summary>
        /// Загружает данные c двоичного файла
        /// </summary>
        internal void Load()
        {
            using (var fs = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                if (fs.Length == 0)
                {
                    return;
                }

                var serializer = new BinaryFormatter();
                Warehouse st = (Warehouse)serializer.Deserialize(fs);
                Copy(st.Products, _warehouse.Products);
                Copy(st.Products, _warehouse.Products);
                Copy(st.Suppliers, _warehouse.Suppliers);
                Copy(st.Supplies, _warehouse.Supplies);
            }

            void Copy<T>(List<T> from, List<T> to)
            {
                to.Clear();
                to.AddRange(from);
            }
        }

        /// <summary>
        /// Сохраняет приходную накладную
        /// </summary>
        /// <param name="supply"></param>
        /// <param name="totalCost"></param>
        internal static void SavePurchaseInvoices(Supply supply)
        {
            if (supply is null)
            {
                throw new ArgumentNullException(nameof(supply));
            }

            int numberFiles = 0;

            if (Directory.Exists(PurchaseInvoicesDirectoryPath))
            {
                numberFiles = new DirectoryInfo(PurchaseInvoicesDirectoryPath).GetFiles().Length;
            }
            else
            {
                Directory.CreateDirectory(PurchaseInvoicesDirectoryPath);
            }

            string path = $"{PurchaseInvoicesDirectoryPath}\\{(numberFiles + 1)}_{supply.Supplier.Name}_{supply.ReceiptDate.ToString("dd-MM-yyyy")}.txt";

            using (StreamWriter wr = new StreamWriter(path))
            {
                wr.WriteLine($"Приходная накладная от {supply.ReceiptDate.ToShortDateString()}");
                wr.WriteLine($"Поставщик: {supply.Supplier.Name}\n");
                wr.WriteLine($"{"№",5}{"Наименование",20}{"Ед. измер.",15}{"Количество",15}{"Цена",15}");

                for (int i = 0; i < supply.Products.Count; i++)
                {
                    Product product = supply.Products[i];
                    wr.WriteLine($"{(i + 1),5}{product.Name,20}{product.Unit,15}{product.Count,15}{product.Price,15}");
                }

                wr.WriteLine($"\nВсего товаров: {supply.Products.Count}");
                wr.WriteLine($"Итоговая стоимость: {supply.TotalCost}\n");
            }
        }

        /// <summary>
        /// Сохраняет расходную накладную
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="products"></param>
        internal static void SaveSalesInvoice(string recipient, List<(Product, int)> products)
        {
            if (string.IsNullOrWhiteSpace(recipient))
            {
                throw new ArgumentNullException(nameof(recipient), "Получатель не может быть null или пустой строкой.");
            }

            if (products is null || products.Count == 0)
            {
                throw new ArgumentNullException(nameof(products), "Список продуктов не может быть пустым или null.");
            }

            int numberFiles = 0;

            if (Directory.Exists(SalesInvoicesDirectoryPath))
            {
                numberFiles = new DirectoryInfo(SalesInvoicesDirectoryPath).GetFiles().Length;
            }
            else
            {
                Directory.CreateDirectory(SalesInvoicesDirectoryPath);
            }

            string path = $"{SalesInvoicesDirectoryPath}\\{(numberFiles + 1)}_{recipient}_{products[0].Item1.ReceiptDate.ToString("dd-MM-yyyy")}.txt";

            using (StreamWriter wr = new StreamWriter(path))
            {
                int counter = 1;
                decimal totalCost = 0;

                wr.WriteLine($"Расходная накладная от {DateTime.Now.ToShortDateString()}");
                wr.WriteLine($"Получатель: {recipient}\n");

                wr.WriteLine($"{"№",5}{"Наименование",20}{"Ед. измерения",15}{"Количество",15}" +
                                  $"{"Цена",15}{"Дата получения",20}{"Поставщик",20}{"Номер телефона",20}");

                foreach ((Product product, int count) in products)
                {

                    wr.WriteLine($"{counter++,5}{product.Name,20}{product.Unit,15}{count,15}" +
                                      $"{product.Price,15}{product.ReceiptDate.ToShortDateString(),20}" +
                                      $"{product.Supply.Supplier.Name,20}{product.Supply.Supplier.PhoneNumber,20}");

                    totalCost += product.Price * count;
                }

                wr.WriteLine($"\nВсего товаров: {products.Count}");
                wr.WriteLine($"Итоговая стоимость: {totalCost}\n");
            }
        }
    }
}
