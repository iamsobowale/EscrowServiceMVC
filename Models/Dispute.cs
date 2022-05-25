using System;
using EscrowService.Auitable;

namespace EscrowService.Models
{
    public class Dispute:BaseEntity
    {
        public int AdminId { get; set; }
        public Admin Admin { get; set; }
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }
        public string Reason { get; set; }
        public string Description { get; set; }
        public bool IsResolved { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}