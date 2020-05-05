using System;
using System.Collections.Generic;

namespace WarehouseLibrary.Models
{
    class Supply
    {
        public Supplier Supplier { get; set; }
        public List<Product> Products { get; set; }

        public Supply(Supplier supplier, List<Product> products)
        {
            Supplier = supplier;
            Products = products;
        }

        public override string ToString()
        {
            return Supplier.Name;
        }
    }
}
