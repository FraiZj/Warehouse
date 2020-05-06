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
            Console.Clear();

            Console.WriteLine("Поставщик: " + supply.Supplier.Name);
            Console.WriteLine("Дата получения: " + supply.ReceiptDate + "\n");

            Console.WriteLine($"{"№",5}{"Наименование",15}{"Ед. измер.",15}{"Количество",15}{"Цена",10}");
            for (var i = 0; i < supply.Products.Count; i++)
            {
                Console.WriteLine($"{(i + 1),5}{supply.Products[i].Name,15}{supply.Products[i].Unit,15}{supply.Products[i].Count,15}{supply.Products[i].Price,10}");
            }

            Console.WriteLine("\nВсего товаров: " + supply.Products.Count + "\n");
        }

        /// <summary>
        /// Выводит список всех товаров на складе
        /// </summary>
        public void PrintAllProducts()
        {
            Console.WriteLine($"{"№",5}{"Наименование",15}{"Ед. измерения",15}{"Количество",15}{"Цена",10}{"Дата получения",20}{"Поставщик",15}{"Номер телефона",20}");
            int counter = 0;
            foreach (var supply in Supplies)
            {
                foreach (var product in supply.Products)
                {
                    Console.WriteLine($"{(counter++),5}{product.Name,15}{product.Unit,15}{product.Count,15}{product.Price,10}" +
                                      $"{product.ReceiptDate.ToShortDateString(),20}{supply.Supplier.Name,15}{supply.Supplier.PhoneNumber,20}");
                }
            }
            Console.WriteLine("\nВсего товаров: " + Products.Count + "\n");
        }

        public void ReturnProducts()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            new Dao(this).Save();
        }

        public void Load()
        {
            new Dao(this).Load();
        }
    }
}
