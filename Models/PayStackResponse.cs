using PayStack.Net;

namespace EscrowService.Models
{
    public class PayStackResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public TransactionInitialize data { get; set; }
   
    }
}