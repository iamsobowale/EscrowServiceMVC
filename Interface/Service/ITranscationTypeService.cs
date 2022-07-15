using System.Collections.Generic;
using System.Threading.Tasks;
using EscrowService.DTO;

namespace EscrowService.Interface.Service
{
    public interface ITranscationTypeService
    {
        Task<TransactionTypeServiceDto> GetTransactionTypeById(int id);
        Task<TransactionTypeServiceDto> GetTransactionTypeByName(string name);
        Task<BaseResponse> CreateTransactionType(CreateTransactionTypeServiceDto transactionType);
        Task<BaseResponse> UpdateTransactionType(UpdateTransactionTypeServiceDto transactionType);
        Task<bool> DeleteTransactionType(int id);
        Task<TransactionTypeListResponseModel> GetAllTransactionTypeByReferenceNumber(string transactionId);
        public Task<BaseResponse> AcceptSubTransaction(string transactionReferenceNumber);
        public Task<TransactionTypeListResponseModel> GetDeliverSubTransaction(string transactionReferenceNumber);
        public Task<BaseResponse> RejectSubTransaction(string transactionReferenceNumber);
        Task<TransactionTypeListResponseModel> GetAcceptedSubTransaction();
        Task<BaseResponse> MakeSubTransactionDone(string transactionReferenceNumber);
        Task<TransactionTypeListResponseModel> GetSubTransactionByTransactionRef(string transactionReference);
    }
}