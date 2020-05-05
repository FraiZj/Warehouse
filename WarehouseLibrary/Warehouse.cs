using System;
using System.Collections.Generic;
using WarehouseLibrary.Models;

namespace WarehouseLibrary
{
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

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Load()
        {
            throw new NotImplementedException();
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
    }
}
