using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using WarehouseLibrary.Models;

namespace WarehouseLibrary.Data
{
    class Dao
    {
        const string FilePath = "warehouse.bin";
        private readonly Warehouse _warehouse;

        public Dao(Warehouse warehouse)
        {
            _warehouse = warehouse;
        }

        public void Save()
        {
            using (var fs = new FileStream(FilePath, FileMode.Create))
            {
                var serializer = new BinaryFormatter();
                serializer.Serialize(fs, _warehouse);
            }
        }

        public void Load()
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
    }
}
