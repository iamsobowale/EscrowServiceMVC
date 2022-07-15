using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EscrowService.Auitable;
using EscrowService.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EscrowService.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public string SellerId { get; set; }
        public decimal TotalPrice { get; set; }
        public string ItemQuantity { get; set; }
        public string ItemTitle { get; set; }
        public string ReferenceNumber { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime DeliveryDate { get; set; }
        [EnumDataType(typeof(TransactionStatus))]
        [JsonConverter(typeof(StringEnumConverter))]
        public TransactionStatus Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public IList<TransactionType> TransactionTypes { get; set; } = new List<TransactionType>();
        public IEnumerable<Message> Messages { get; set; } = new List<Message>();
        public IEnumerable<Payment> Payments { get; set; } = new List<Payment>();
        public IEnumerable<Dispute> Disputes { get; set; } = new List<Dispute>();
        public IList<TradersTransaction> TradersTransactions { get; set; } = new List<TradersTransaction>();
    }
}

