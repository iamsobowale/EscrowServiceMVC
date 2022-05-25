

using EscrowService.Auitable;

namespace EscrowService.Models
{
    public class TradersTransaction:BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }
    }
}
