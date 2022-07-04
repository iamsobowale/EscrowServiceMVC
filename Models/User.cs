using System.ComponentModel.DataAnnotations;
using EscrowService.Auitable;
using EscrowService.Models;
using EscrowService.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EscrowService.Models
{
    public class User:BaseEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        [EnumDataType(typeof(TransactionStatus))]
        [JsonConverter(typeof(StringEnumConverter))]
        public Role Role { get; set; }
        public Trader Trader{get;set;}
        public Admin Admin { get; set; }
    }    
}
