using System;

namespace WarehouseLibrary.Models
{
    [Serializable]
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address  { get; set; }

        public Supplier(string name, string phoneNumber, string address)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "Имя не может быть null или пустой строкой.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "Номер телефона не может быть null или пустой строкой.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "Адрес не может быть null или пустой строкой.");

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
