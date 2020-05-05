using System;
using System.Collections.Generic;
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

        public void AddSupply(Supply supply)
        {
            Products.AddRange(supply.Products);
            Suppliers.Add(supply.Supplier);
            Supplies.Add(supply);
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
