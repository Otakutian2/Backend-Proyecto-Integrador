namespace proyecto_backend.Dto
{
    public class DishOrderStatistics
    {
        public string DishId { get; set; }
        public string Name { get; set; }
        public string ImgDish { get; set; }
        public string Category { get; set; }
        public decimal TotalSales { get; set; }
        public int QuantityOfDishesSold { get; set; }
    }
}
