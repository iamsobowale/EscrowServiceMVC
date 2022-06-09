using System;

namespace EscrowService.Models
{
    public class TransactionType
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DeliveryDate { get; set; }
        public decimal Price { get; set; }
        public bool IsPaidOut { get; set; }
        public TransactionTypeEnum Status { get; set; }
        public string Reference { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}