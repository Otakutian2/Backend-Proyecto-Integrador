namespace project_backend.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Dni { get; set; }
        public List<Voucher> Vouchers { get; } = new List<Voucher>();
    }
}
