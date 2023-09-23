namespace project_backend.Dto
{
    public class OrderDetail
    {
        public int Quantity { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string UnitOfMeasure { get; set; } = "UNIDADES";
        public decimal Amount { get; set; }
    }
}
