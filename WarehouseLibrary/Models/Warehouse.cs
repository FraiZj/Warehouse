using System;
using System.Collections.Generic;
using WarehouseLibrary.Data;

namespace WarehouseLibrary.Models
{
    [Serializable]
    public class Warehouse
    {
        private readonly string _productInfo = $"{"№",5}{"Наименование",15}{"Ед. измер.",10}{"Количество",10}{"Цена",10}";

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

            Console.WriteLine(_productInfo);
            for (var i = 0; i < supply.Products.Count; i++)
            {
                Console.WriteLine($"{(i + 1),5}{supply.Products[i].Name,15}{supply.Products[i].Unit,10}{supply.Products[i].Count,10}{supply.Products[i].Price,10}");
            }

            Console.WriteLine("\nВсего товаров: " + supply.Products.Count + "\n");
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
