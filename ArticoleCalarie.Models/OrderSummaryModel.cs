using ArticoleCalarie.Models.Enums;

namespace ArticoleCalarie.Models
{
    public class OrderSummaryModel
    {
        public int OrderNumber { get; set; }

        public string Email { get; set; }

        public string Products { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatusViewEnum OrderStatus { get; set; }
    }
}
