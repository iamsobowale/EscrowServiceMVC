using EscrowService.Auitable;

namespace EscrowService.Models
{
    public class PaymentMethod:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

