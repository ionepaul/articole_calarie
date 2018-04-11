namespace ArticoleCalarie.Models
{
    public class ProductListViewItemModel
    {
        public int Id { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool IsNew { get; set; }

        public bool IsOnSale { get; set; }

        public decimal PriceAfterSaleApplied { get; set; }

        public string ProductImageName { get; set; }

        public int SalePercentage { get; set; }

        public string SubcategoryName { get; set; }

        public string CategoryName { get; set; }

        public int? SubcategoryId { get; set; }
    }
}
