namespace proyecto_backend.Dto
{
    public class SalesDataPerDate
    {
        public DateTime CreatedAt { get; set; }
        public decimal AccumulatedSales { get; set; }
        public int NumberOfGeneratedReceipts { get; set; }
        public int QuantityOfDishSales { get; set; }
        public string BestSellingDish { get; set; }
    }
}
