using System;
using System.Collections.Generic;

namespace WarehouseLibrary.Models
{
    [Serializable]
    public class Supply
    {
        public Supplier Supplier { get; private set; }
        public List<Product> Products { get; private set; }
        public DateTime ReceiptDate { get; set; }

        public Supply(Supplier supplier, List<Product> products)
        {
            Supplier = supplier;
            Products = products;
            ReceiptDate = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{Supplier.Name,15}{Products.Count,10}";
        }
    }
}
