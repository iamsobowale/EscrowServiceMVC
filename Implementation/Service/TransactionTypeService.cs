using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EscrowService.DTO;
using EscrowService.Interface.Repository;
using EscrowService.Interface.Service;
using EscrowService.Models;
using Microsoft.AspNetCore.Mvc;

namespace EscrowService.Implementation.Service
{
    public class TransactionTypeService:ITranscationTypeService
    {
        private readonly ITransactionTypeRepo _transactionTypeRepo;
        private readonly ITransactionRepo _transactionRepo;

        public TransactionTypeService(ITransactionTypeRepo transactionTypeRepo, ITransactionRepo transactionRepo)
        {
            _transactionTypeRepo = transactionTypeRepo;
            _transactionRepo = transactionRepo;
        }

        public Task<TransactionTypeServiceDto> GetTransactionTypeById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<TransactionTypeServiceDto> GetTransactionTypeByName(string name)
        {
            throw new System.NotImplementedException();
        }

        public async Task<BaseResponse> CreateTransactionType(IList<CreateTransactionTypeServiceDto> transactionType, string transactionReferenceNumber)
        {
            var getTransction = await _transactionRepo.GetTransactionByReferenceNumber(transactionReferenceNumber);
            if (getTransction == null)
            {
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Transaction not found"
                };
            }
            if (getTransction.TransactionTypes.Count==5)
            {
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "You can't add more than 5 transaction types"
                };
            }
            var createTransactionTypes = new TransactionType()
            {
                Name = transactionType[0].Name,
                Description = transactionType[0].Description,
                CreatedDate = DateTime.Now,
                TransactionId = getTransction.Id,
                Price = transactionType[0].Price
            };
            var createTransactionType = _transactionTypeRepo.CreateTransactionType(createTransactionTypes);
            if (createTransactionType == null)
            {
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Transaction type not created"
                };
            }
            var sum = transactionType.Sum(x => x.Price);
            getTransction.TotalPrice = sum;
            var updateTransaction = _transactionRepo.UpdateTransaction(getTransction);
            if (updateTransaction == null)
            {
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Transaction not updated"
                };
            }
            return new BaseResponse
            {
                IsSuccess = true,
                Message = "Transaction type created"
            };
        }

        public Task<BaseResponse> UpdateTransactionType(UpdateTransactionTypeServiceDto transactionType)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteTransactionType(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<TransactionTypeListResponseModel> GetAllTransactionTypeByReferenceNumber(string transactionId)
        {
            var getTransaction = await _transactionRepo.GetTransactionByReferenceNumber(transactionId);
            if (getTransaction == null)
            {
                return new TransactionTypeListResponseModel()
                {
                    Message = "Transaction not found",
                    IsSuccess = false
                };
            }
            var getTransactionTypes = await _transactionTypeRepo.GetAllTransactionTypeByReferenceNumber(getTransaction.Id);
            return new TransactionTypeListResponseModel()
            {
                Message = "Transaction type found",
                IsSuccess = true,
                Transaction = getTransactionTypes.Select(c => new TransactionTypeServiceDto()
                {
                    Name = c.Name,
                    TransactionReferenceNumber = c.Reference,
                    Description = c.Description,
                    Price = c.Price,
                    Status = c.Status,
                    CreatedDate = c.CreatedDate
                }).ToList()
            };
        }

        public async Task<BaseResponse> AcceptSubTransaction(string transactionReferenceNumber)
        {
            
            var getTransactionType = await _transactionTypeRepo.GetTransactionTypeByRefrenceName(transactionReferenceNumber);
            if (getTransactionType == null)
            {
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Sub-Transaction not found"
                };
            }
            getTransactionType.Status = TransactionTypeEnum.Accpeted;
            var updateTransactionType = await _transactionTypeRepo.UpdateTransactionType(getTransactionType);
           
            if (updateTransactionType == null)
            {
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Sub-Transaction not updated"
                };
            }
            bool isAccepted = updateTransactionType.Status == TransactionTypeEnum.Accpeted;
            return new BaseResponse
            {
                IsSuccess = isAccepted,
                Message = "Sub-Transaction Accepted"
            };
           
        }

        public async Task<TransactionTypeListResponseModel> GetDeliverSubTransaction(string transactionReferenceNumber)
        {
            var getdelievered = await _transactionTypeRepo.GetDeliveredSubTransaction(transactionReferenceNumber);
            return new TransactionTypeListResponseModel()
            {
                Transaction = getdelievered.Select(c => new TransactionTypeServiceDto()
                {
                    Description = c.Description,
                    Name = c.Name,
                    Price = c.Price,
                    TransactionReferenceNumber = c.Transaction.ReferenceNumber,
                    Status = c.Status,
                    Reference = c.Reference
                }).ToList(),
                Message = "Found",
                IsSuccess = true
            };
        }
        public async Task<BaseResponse> RejectSubTransaction(string transactionReferenceNumber)
        {
            var getTransactionType = await _transactionTypeRepo.GetTransactionTypeByRefrenceName(transactionReferenceNumber);
            if (getTransactionType == null)
            {
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "TSub-Transaction not found"
                };
            }
            getTransactionType.Status = TransactionTypeEnum.Active;
            var updateTransactionType = await _transactionTypeRepo.UpdateTransactionType(getTransactionType);
            if (updateTransactionType == null)
            {
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Sub-Transaction not updated"
                };
            }
            bool isRejected = updateTransactionType.Status == TransactionTypeEnum.Rejected;
            return new BaseResponse
            {
                IsSuccess = isRejected,
                Message = "Sub-TransactionRejected"
            };
        }

        public async Task<TransactionTypeListResponseModel> GetAcceptedSubTransaction()
        {
            var getdelievered = await _transactionTypeRepo.GetAcceptedSubTransaction();
            return new TransactionTypeListResponseModel()
            {
                Transaction = getdelievered.Select(c => new TransactionTypeServiceDto()
                {
                    Description = c.Description,
                    Name = c.Name,
                    Price = c.Price,
                    TransactionReferenceNumber = c.Transaction.ReferenceNumber,
                    Status = c.Status,
                    Reference = c.Reference,
                    SellerId = c.Transaction.SellerId
                }).ToList(),
                Message = "Found",
                IsSuccess = true
            };
        }
    }
}