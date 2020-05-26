using System;
using System.Collections.Generic;

namespace WarehouseLibrary.Models
{
    [Serializable]
    public class Supplier
    {
        public string Name { get; internal set; }
        public string PhoneNumber { get; internal set; }
        public string Address  { get; internal set; }
        public int SuppliesCount { get; internal set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса Supplier
        /// </summary>
        public Supplier(string name, string phoneNumber, string address)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "Имя не может быть null или пустой строкой.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "Номер телефона не может быть null или пустой строкой.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "Адрес не может быть null или пустой строкой.");
            }

            Name = name;
            PhoneNumber = phoneNumber;
            Address = address;
            SuppliesCount = 0;
        }

        public void AddSupliesCount()
        {
            SuppliesCount++;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
