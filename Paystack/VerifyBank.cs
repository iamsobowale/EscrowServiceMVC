namespace EscrowService.Models
{
    public class VerifyBank
    {
        public bool status { get; set; }
        public string message { get; set; }
        public VerifyBankData data { get; set; }

    }
}