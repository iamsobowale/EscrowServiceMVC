using EscrowService.Auitable;
using EscrowService.Models;
using EscrowService.Models;

namespace EscrowService.Models
{
    public class User:BaseEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public Trader Trader{get;set;}
        public Admin Admin { get; set; }
    }    
}
