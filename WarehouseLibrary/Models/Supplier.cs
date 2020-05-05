namespace WarehouseLibrary.Models
{
    class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address  { get; set; }

        public Supplier()
        {
            
        }

        public Supplier(string name, string phoneNumber, string address)
        {
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
