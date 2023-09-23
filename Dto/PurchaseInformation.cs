namespace project_backend.Dto
{
    public class PurchaseInformation
    {
        public string PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CurrencyType { get; set; } = "Soles";
    }
}
