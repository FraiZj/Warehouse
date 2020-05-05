using System;
using System.Collections.Generic;
using WarehouseLibrary.Models;

namespace WarehouseLibrary
{
    class Warehouse
    {
        public List<Product> Products { get; set; }
        public List<Supplier> Suppliers { get; set; }
        public List<Supply> Supplies { get; set; }

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

        public void AddSupply()
        {
            throw new NotImplementedException();
        }

        public void ReturnProducts()
        {
            throw new NotImplementedException();
        }
    }
}
