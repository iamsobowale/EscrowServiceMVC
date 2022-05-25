
using System;
using EscrowService.Models;

namespace EscrowService.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string MessageText { get; set; }
        public DateTime Date { get; set; }
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }
    } 
}
