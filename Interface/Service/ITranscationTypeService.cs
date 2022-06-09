using System.Collections.Generic;
using System.Threading.Tasks;
using EscrowService.DTO;

namespace EscrowService.Interface.Service
{
    public interface ITranscationTypeService
    {
        Task<TransactionTypeServiceDto> GetTransactionTypeById(int id);
        Task<TransactionTypeServiceDto> GetTransactionTypeByName(string name);
        Task<BaseResponse> CreateTransactionType(IList<CreateTransactionTypeServiceDto> transactionType, string transactionReferenceNumber);
        Task<BaseResponse> UpdateTransactionType(UpdateTransactionTypeServiceDto transactionType);
        Task<bool> DeleteTransactionType(int id);
        Task<TransactionTypeListResponseModel> GetAllTransactionTypeByReferenceNumber(string transactionId);
    }
}