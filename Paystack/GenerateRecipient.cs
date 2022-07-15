namespace EscrowService.Models
{
    public class GenerateRecipient
    {
        public bool status { get; set; }
        public string message { get; set; }
        
        public GenerateRecipientData data { get; set; }
    }
}