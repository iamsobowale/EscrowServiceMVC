using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EscrowService.DTO;
using EscrowService.Interface.Repository;
using EscrowService.Interface.Service;
using EscrowService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EscrowService.Implementation.Service
{
    public class TransactionService:ITransactionService
    {
        private readonly ITransactionRepo _transactionRepo;
        private readonly ITraderRepo _traderRepo;
        public TransactionService(ITransactionRepo transactionRepo, ITraderRepo traderRepo)
        {
            _transactionRepo = transactionRepo;
            _traderRepo = traderRepo;
        }

        public async Task<BaseResponse> CreateTransaction(CreateTransactionDto transaction)
        {
            var findBuyer = await _traderRepo.GetTraderByEmailAsync(transaction.BuyerId);
            if (findBuyer== null)
            {
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }
            var findSeller = await _traderRepo.GetTraderByEmailAsync(transaction.SellerId);
            if (findSeller == null)
            {
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }
            var genertateReferencenumber = $"Ref{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5).ToUpper()}";

            var transactionResponse = new Transaction()
            {
                BuyerId = findBuyer.Email,
                SellerId = findSeller.Email,
                Status = TransactionStatus.isIntialized,
                CreatedDate = DateTime.UtcNow,
                ItemName = transaction.ItemName,
                ItemPrice = transaction.ItemPrice,
                ItemQuantity = transaction.ItemQuantity,
                ItemTitle = transaction.ItemTitle,
                ItemDescription = transaction.ItemDescription,
                ReferenceNumber = genertateReferencenumber,
                DeliveryDate = transaction.DeliveryDate,
                DeliveryAddress = transaction.DeliveryAddress,
            };
            var createTransaction = await _transactionRepo.CreatTransaction(transactionResponse);
            if (createTransaction==null)
            {
                return new BaseResponse()
                {
                    IsSuccess = false,
                    Message = "Transaction Failed"
                };
            }

            var traderTransaction = new TradersTransaction()
            {
                BuyerId = findBuyer.Id,
                SellerId = findSeller.Id,
                TransactionId = transactionResponse.Id,
            };
            
            createTransaction.TradersTransactions.Add(traderTransaction);
            var createjoin = await _transactionRepo.UpdateTransaction(transactionResponse);
            if (createjoin==null)
            {
                return new BaseResponse()
                {
                    IsSuccess = false,
                    Message = "Transaction Failed"
                };
            }
            return new BaseResponse()
            {
                IsSuccess = true,
                Message = "Transaction Success"
            };
        }

        public async Task<TransactionResponseModel> GetTransaction(int transactionId)
        {
           var getTransaction = await _transactionRepo.GetTransaction(transactionId);
            if (getTransaction==null)
            {
                return new TransactionResponseModel()
                {
                    IsSuccess = false,
                    Message = "Transaction not found"
                };
            }
            return new TransactionResponseModel()
            {
                IsSuccess = true,
                Message = "Transaction found",
                Transaction = new TransactionDto()
                {
                    reference_id = getTransaction.ReferenceNumber,
                    transaction_status = getTransaction.Status,
                    BuyerId = getTransaction.BuyerId,
                    DeliveryAddress = getTransaction.DeliveryAddress,
                    DeliveryDate = getTransaction.DeliveryDate,
                    ItemDescription = getTransaction.ItemDescription,
                    ItemName = getTransaction.ItemName,
                    ItemPrice = getTransaction.ItemPrice,
                    ItemQuantity = getTransaction.ItemQuantity,
                    ItemTitle = getTransaction.ItemTitle,
                    SellerId = getTransaction.SellerId,
                    CreatedDate = getTransaction.CreatedDate,
                }
            };  
        }
        public async Task<TransactionListResponseModel> GetAllTransaction()
        {
          var getAll = await _transactionRepo.GetAllTransaction();
          return new TransactionListResponseModel()
          {
              TransactionList = getAll.Select(getTransaction => new TransactionDto()
              {
                  reference_id = getTransaction.ReferenceNumber,
                  transaction_status = getTransaction.Status,
                  BuyerId = getTransaction.BuyerId,
                  DeliveryAddress = getTransaction.DeliveryAddress,
                  DeliveryDate = getTransaction.DeliveryDate,
                  ItemDescription = getTransaction.ItemDescription,
                  ItemName = getTransaction.ItemName,
                  ItemPrice = getTransaction.ItemPrice,
                  ItemQuantity = getTransaction.ItemQuantity,
                  ItemTitle = getTransaction.ItemTitle,
                  SellerId = getTransaction.SellerId,
                  CreatedDate = getTransaction.CreatedDate,
              }).ToList(),
              Message = "Transaction found",
              IsSuccess = true
          };

        }

        public async Task<TransactionResponseModel> GetTransactionByReferenceNumber(string referenceNumber)
        {
            var getTransaction = await _transactionRepo.GetTransactionByReferenceNumber(referenceNumber);
            if (getTransaction==null)
            {
                return new TransactionResponseModel()
                {
                    IsSuccess = false,
                    Message = "Transaction not found"
                };
            }

            return new TransactionResponseModel()
            {
                IsSuccess = true,
                Message = "Transaction found",
                Transaction = new TransactionDto()
                {
                    reference_id = getTransaction.ReferenceNumber,
                    transaction_status = getTransaction.Status,
                    BuyerId = getTransaction.BuyerId,
                    DeliveryAddress = getTransaction.DeliveryAddress,
                    DeliveryDate = getTransaction.DeliveryDate,
                    ItemDescription = getTransaction.ItemDescription,
                    ItemName = getTransaction.ItemName,
                    ItemPrice = getTransaction.ItemPrice,
                    ItemQuantity = getTransaction.ItemQuantity,
                    ItemTitle = getTransaction.ItemTitle,
                    SellerId = getTransaction.SellerId,
                    CreatedDate = getTransaction.CreatedDate,
                }
            };
        }

        public async Task<TransactionListResponseModel> GetAllTransactionsByTraderEmail(string email)
        {
            var getAll = await _transactionRepo.GetAllTransactionsByTraderEmail(email);
           
            return new TransactionListResponseModel()
            {
                TransactionList = getAll.Select(getTransaction => new TransactionDto()
                {
                    reference_id = getTransaction.ReferenceNumber,
                    transaction_status = getTransaction.Status,
                    BuyerId = getTransaction.BuyerId,
                    DeliveryAddress = getTransaction.DeliveryAddress,
                    DeliveryDate = getTransaction.DeliveryDate,
                    ItemDescription = getTransaction.ItemDescription,
                    ItemName = getTransaction.ItemName,
                    ItemPrice = getTransaction.ItemPrice,
                    ItemQuantity = getTransaction.ItemQuantity,
                    ItemTitle = getTransaction.ItemTitle,
                    SellerId = getTransaction.SellerId,
                    CreatedDate = getTransaction.CreatedDate,
                }).ToList(),
                Message = "Transaction found",
                IsSuccess = true
            };
            
        }

        public Task<TransactionListResponseModel> GetAllTradersInTransaction(string referenceNumber)
        {
            throw new System.NotImplementedException();
        }

        public async Task<TransactionListResponseModel> GetAllTransactionByTransactionStatus(TransactionStatus status, string email)
        {
           var getTransactionStatus = await _transactionRepo.GetAllTransactionByTransactionStatus(status, email);
           return new TransactionListResponseModel()
           {
               TransactionList = getTransactionStatus.Select(getTransaction => new TransactionDto()
               {
                   reference_id = getTransaction.ReferenceNumber,
                   transaction_status = getTransaction.Status,
                   BuyerId = getTransaction.BuyerId,
                   DeliveryAddress = getTransaction.DeliveryAddress,
                   DeliveryDate = getTransaction.DeliveryDate,
                   ItemDescription = getTransaction.ItemDescription,
                   ItemName = getTransaction.ItemName,
                   ItemPrice = getTransaction.ItemPrice,
                   ItemQuantity = getTransaction.ItemQuantity,
                   ItemTitle = getTransaction.ItemTitle,
                   SellerId = getTransaction.SellerId,
                   CreatedDate = getTransaction.CreatedDate,
               }).ToList(),
               Message = "Transaction found",
               IsSuccess = true
           };

        }

        public Task<TransactionResponseModel> GetTransactionByTransactionStatus(TransactionStatus status)
        {
            throw new System.NotImplementedException();
        }

        public async Task<BaseResponse> ConFirmTransaction(string transactionId, string email)
        {
            var getTransaction = await _transactionRepo.GetTransactionByReferenceNumber(transactionId);
            if (getTransaction==null)
            {
                return new BaseResponse()
                {
                    IsSuccess = false,
                    Message = "Transaction not found"
                };
            }
            if (getTransaction.Status==TransactionStatus.isAgreed)
            {
                return new BaseResponse()
                {
                    IsSuccess = false,
                    Message = "Transaction Terms Already Accepted"
                };
            }

            if (getTransaction.Status!=TransactionStatus.isIntialized)
            {
                return new BaseResponse()
                {
                    Message = "Transaction not in initialized status",
                    IsSuccess = false
                };
            }
            var getTrader = await _traderRepo.GetTraderByEmailAsync(email);
            if (getTrader.Email==getTransaction.BuyerId)
            {
                return new BaseResponse()
                {
                    IsSuccess = false,
                    Message = "Trader Cant confirm this transaction"
                };
            }
            var get = getTransaction.Status = TransactionStatus.isAgreed;
            var updateTransaction = await _transactionRepo.UpdateTransaction(getTransaction);
            if (updateTransaction==null)
            {
                return new BaseResponse()
                {
                    IsSuccess = false,
                    Message = "Transaction not confirmed"
                };
            }
            return new BaseResponse()
            {
                IsSuccess = true,
                Message = "Transaction confirmed"
            };
        }

        public async Task<BaseResponse> CancelTransaction(string transactionId, string email)
        {
            var getTransaction = await _transactionRepo.GetTransactionByReferenceNumber(transactionId);
            if (getTransaction==null)
            {
                return new BaseResponse()
                {
                    IsSuccess = false,
                    Message = "Transaction not found"
                };
            }
            if (getTransaction.Status==TransactionStatus.IsCancelled)
            {
                return new BaseResponse()
                {
                    IsSuccess = false,
                    Message = "Transaction already confirmed"
                };
            }
            var getTrader = await _traderRepo.GetTraderByEmailAsync(email);
            if (getTrader.Email== getTransaction.BuyerId)
            {
                if (getTransaction.Status == TransactionStatus.isIntialized)
                {
                    var get = getTransaction.Status = TransactionStatus.IsCancelled;
                    var updateTransaction = await _transactionRepo.UpdateTransaction(getTransaction);
                    return new BaseResponse()
                    {
                        IsSuccess = true,
                        Message = "Transaction Cancelled"
                    };
                }

                return new BaseResponse()
                {
                    Message = "You Cant Cancel Transaction",
                    IsSuccess = false
                };
            }
            var changeStatus = getTransaction.Status = TransactionStatus.IsCancelled;
            var update = await _transactionRepo.UpdateTransaction(getTransaction);
            if (update==null)
            {
                return new BaseResponse()
                {
                    Message = "Cancellation Failed",
                    IsSuccess = false
                };
            }
            return new BaseResponse()
            {
                Message = "Cancellation Done",
                IsSuccess = true
            };
        }

        public async Task<BaseResponse> ProcessTransaction(string transactionId, string email)
        {
            var getTransaction = await _transactionRepo.GetTransactionByReferenceNumber(transactionId);
            if (getTransaction==null)
            {
                return new BaseResponse()
                {
                    Message = "Transaction Not Found",
                    IsSuccess = false
                };
            }

            var getTrader = await _traderRepo.GetTraderByEmailAsync(email);
            if (getTrader.Email==getTransaction.BuyerId)
            {
                return new BaseResponse()
                {
                    Message = "Youre not Allowed To Process As A buyer",
                    IsSuccess = false
                };
            }

            if (getTransaction.Status ==TransactionStatus.isActive)
            {
               var change = getTransaction.Status = TransactionStatus.isProcessing;
              await _transactionRepo.UpdateTransaction(getTransaction);
              return new BaseResponse()
              {
                  Message = "Transaction Now Processing",
                  IsSuccess = true
              };
            }
            return new BaseResponse()
            {
                Message = "Transaction Cant Be Processed Either Because It Has Been Processed or Not Agreed",
                IsSuccess = false
            };
        }
    }
}