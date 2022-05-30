

using EscrowService.Auitable;

namespace EscrowService.Models
{
    public class TradersTransaction
    {
        public int TradersTransactionId { get; set; }
        public int SellerId { get; set; }
        public int BuyerId { get; set; }
        public Trader Trader { get; set; }
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }
    }
}
