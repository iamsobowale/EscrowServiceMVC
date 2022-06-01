namespace EscrowService.Models
{
    public enum TransactionStatus
    {
        isIntialized=1,
        isAgreed,
        isActive,
        isProcessing,
        IsDelivered,
        IsRejected,
        IsCancelled,
        IsReceived,
        IsReturned,
        IsReturnedToSeller, 
        IsCompleted,
    }
}