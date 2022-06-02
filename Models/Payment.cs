using System;
using EscrowService.Auitable;
using EscrowService.Models;

namespace EscrowService.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public decimal Amount { get; set; }


        public string ReferenceNumber {get;set;}
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public PaymentStatus Status { get; set; }
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }
    } 
}
