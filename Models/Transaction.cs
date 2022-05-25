

using System;
using System.Collections.Generic;
using EscrowService.Auitable;
using EscrowService.Models;

namespace EscrowService.Models
{
    public class Transaction:BaseEntity
    {
        public string BuyerId { get; set; }
        public string SellerId { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; } 
        public decimal ItemPrice { get; set; }
        public string ItemQuantity { get; set; }
        public string ItemDoc { get; set; }
        public TransactionStatus Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public IEnumerable<Message> Messages { get; set; } = new List<Message>();
        public IEnumerable<Payment> Payments { get; set; }
        public IEnumerable<Dispute> Disputes { get; set; }
        public IEnumerable<TradersTransaction> TradersTransactions { get; set; } = new List<TradersTransaction>();
    }
}

