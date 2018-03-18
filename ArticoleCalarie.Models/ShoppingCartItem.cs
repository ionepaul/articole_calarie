namespace ArticoleCalarie.Models
{
    public class ShoppingCartItem
    {
        public string ImageName { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string Size { get; set; }

        public string Color { get; set; }

        public string ProductCode { get; set; }

        public int SalePercentage { get; set; }

        public string ProductCategoryName { get; set; }

        public string ProductSubcategoryName { get; set; }

        public string ProductSubcategoryId { get; set; }
    }
}
