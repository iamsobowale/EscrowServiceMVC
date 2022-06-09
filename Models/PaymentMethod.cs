using EscrowService.Auitable;

namespace EscrowService.Models
{
    public class PaymentMethod:BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

