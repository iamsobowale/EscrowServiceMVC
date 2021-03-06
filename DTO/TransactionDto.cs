using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EscrowService.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace EscrowService.DTO
{
    public class TransactionDto
    {
        public string reference_id { get; set; }
        [EnumDataType(typeof(TransactionStatus))]
        [JsonConverter(typeof(StringEnumConverter))]
        public TransactionStatus transaction_status { get; set; } 
        public string BuyerId { get; set; }
        public decimal TotalPrice { get; set; }
        public string DeliveryAddress { get; set; }
        public int DeliveryDate { get; set; }
        public string ItemTitle { get; set; }
        public string SellerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int numberOfSub { get; set; }
        public string ItemQuantity { get; set; }
    }
    public class CreateTransactionDto
    {
        public int DeliveryDate { get; set; }
        public string BuyerId { get; set; }
        public string DeliveryAddress { get; set; }
        public string ItemTitle { get; set; }
        public string SellerId { get; set; }
        public decimal Price { get; set; }
    }

    public class UpdateTransaction
    {
        public string BuyerId { get; set; }
        public string SellerId { get; set; }
    }

    public class TransactionResponseModel:BaseResponse
    {
        public TransactionDto Transaction { get; set; }
    }
    public class TransactionListResponseModel:BaseResponse
    {
        public IEnumerable<TransactionDto> TransactionList { get; set; }
    }
}