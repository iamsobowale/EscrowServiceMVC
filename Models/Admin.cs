using System;
using System.Collections.Generic;
using EscrowService.Auitable;

namespace EscrowService.Models
{
    public class Admin:BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string AdminId { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public DateTime Dob { get; set; }
        public int UserId { get; set; }
        public Role Role { get; set; }
        public User User { get; set; }
        public IEnumerable<Dispute> Disputes { get; set; }
    }
}