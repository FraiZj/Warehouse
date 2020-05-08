using System;

namespace WarehouseLibrary.Models
{
    [Serializable]
    public class Supplier
    {
        public string Name { get; }
        public string PhoneNumber { get; }
        public string Address  { get; }

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
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
