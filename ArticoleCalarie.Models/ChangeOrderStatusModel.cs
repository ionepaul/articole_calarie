using ArticoleCalarie.Models.Enums;

namespace ArticoleCalarie.Models
{
    public class ChangeOrderStatusModel
    {
        public int OrderNumber { get; set; }

        public OrderStatusViewEnum NewOrderStatus { get; set; }

        public string DeliveryTime { get; set; }
    }
}
