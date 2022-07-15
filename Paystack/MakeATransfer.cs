namespace EscrowService.Models
{
    public class MakeATransfer
    {
        public bool status { get; set; }
        public string message { get; set; }
        public MakeATransferData data { get; set; }
    }
}