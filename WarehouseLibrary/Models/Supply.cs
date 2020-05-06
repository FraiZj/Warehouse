using System;
using System.Collections.Generic;
using System.Linq;

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
            if (supplier is null)
            {
                throw new ArgumentNullException(nameof(supplier), "Поставщик не может быть null.");
            }

            if (products is null || products.Count == 0)
            {
                throw new ArgumentNullException(nameof(products), "Список продуктов не может быть пустым или null.");
            }

            Supplier = supplier;

            foreach (var product in products)
            {
                product.Supply = this;
            }

            Products = products;
            ReceiptDate = DateTime.Now;
        }

        public override string ToString()
        {
            return Supplier.Name;
        }
    }
}
