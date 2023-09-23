namespace project_backend.Dto
{
    public class DishOrderStatistics
    {
        public string DishId { get; set; }
        public string NameDish { get; set; }
        public string ImgDish { get; set; }
        public string Name { get; set; }
        public decimal TotalSales { get; set; }
        public int QuantityOfDishesSold { get; set; }
    }
}
