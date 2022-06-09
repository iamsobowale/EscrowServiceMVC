using System;
using System.Collections.Generic;
using EscrowService.Models;

namespace EscrowService.DTO
{
    public class TransactionTypeServiceDto
    {
        public int TransactionId { get; set; }
        public string  TransactionReferenceNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsPaidOut { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class CreateTransactionTypeServiceDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int DeliveryDate { get; set; }
        public decimal Price { get; set; }
        public bool IsPaidOut { get; set; } = false;
        public DateTime CreatedDate { get; set; }
        
    }
    public class UpdateTransactionTypeServiceDto
    {
        public int TransactionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsPaidOut { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class TransactionTypeResponseModel:BaseResponse
    {
        public TransactionTypeServiceDto Transaction { get; set; }
    }
    public class TransactionTypeListResponseModel:BaseResponse
    {
        public IList<TransactionTypeServiceDto> Transaction { get; set; }
    }
}