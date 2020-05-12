using System;
using System.Collections.Generic;

namespace WarehouseLibrary.Models
{
    [Serializable]
    public class Supply
    {
        public Supplier Supplier { get; internal set;  }
        public List<Product> Products { get; internal set; }
        public DateTime ReceiptDate { get; internal set; }
        public decimal TotalCost { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса Supply
        /// </summary>
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

            foreach (Product product in products)
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
