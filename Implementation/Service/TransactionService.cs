using System;
using System.Collections;
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
        private readonly IPaymentRepo _paymentRepo;
        private readonly ITransactionRepo _transactionRepo;
        private readonly ITraderRepo _traderRepo;
        private readonly ITransactionTypeRepo _transactionTypeRepo;

        public TransactionService(IPaymentRepo paymentRepo, ITransactionRepo transactionRepo, ITraderRepo traderRepo, ITransactionTypeRepo transactionTypeRepo)
        {
            _paymentRepo = paymentRepo;
            _transactionRepo = transactionRepo;
            _traderRepo = traderRepo;
            _transactionTypeRepo = transactionTypeRepo;
        }

        public async Task<BaseResponse> CreateTransaction(CreateTransactionDto transaction)
        {
            List<TransactionType> transactionTypess = new List<TransactionType>();
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
            var generateReferencenumber = $"Ref{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5).ToUpper()}";

            var transactionResponse = new Transaction()
            {
                BuyerId = findBuyer.Email,
                SellerId = findSeller.Email,
                Status = TransactionStatus.isIntialized,
                CreatedDate = DateTime.UtcNow,
                ItemQuantity = transaction.ItemQuantity,
                ItemTitle = transaction.ItemTitle,
                ReferenceNumber = generateReferencenumber,
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
            var createjoinertable = await _transactionRepo.UpdateTransaction(transactionResponse);
            if (createjoinertable==null)
            {
                return new BaseResponse()
                {
                    IsSuccess = false,
                    Message = "Transaction Failed"
                };
            }

            if (transaction.TransactionTypeServices.Count==5)
            {
                
            }
            foreach (var item in transaction.TransactionTypeServices)
            {
                var createTransactionTypes = new TransactionType()
                {
                    Name = item.Name,
                    Description = item.Description,
                    Status = TransactionTypeEnum.Active,
                    CreatedDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(item.DeliveryDate),
                    TransactionId = transactionResponse.Id,
                    Price = item.Price
                };
                var createTransactionType = _transactionTypeRepo.CreateTransactionType(createTransactionTypes);
                if (createTransactionType == null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Transaction item failed to add"
                    };
                }
            }
            var sum = transaction.TransactionTypeServices.Sum(x => x.Price);
            transactionResponse.TotalPrice = sum;
            _transactionRepo.UpdateTransaction(transactionResponse); 
            return new BaseResponse
            {
                IsSuccess = true,
                Message = "Transaction created Successfully"
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
                    ItemQuantity = getTransaction.ItemQuantity,
                    ItemTitle = getTransaction.ItemTitle,
                    SellerId = getTransaction.SellerId,
                    TotalPrice = getTransaction.TotalPrice,
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
                  ItemQuantity = getTransaction.ItemQuantity,
                  ItemTitle = getTransaction.ItemTitle,
                  SellerId = getTransaction.SellerId,
                  TotalPrice = getTransaction.TotalPrice,
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
                    ItemQuantity = getTransaction.ItemQuantity,
                    ItemTitle = getTransaction.ItemTitle,
                    TotalPrice = getTransaction.TotalPrice,
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
                    ItemQuantity = getTransaction.ItemQuantity,
                    ItemTitle = getTransaction.ItemTitle,
                    SellerId = getTransaction.SellerId,
                    CreatedDate = getTransaction.CreatedDate,
                    TotalPrice = getTransaction.TotalPrice,
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
                   TotalPrice = getTransaction.TotalPrice,
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
            // var getTrader = await _traderRepo.GetTraderByEmailAsync(email);
            // if (getTrader.Email==getTransaction.BuyerId)
            // {
            //     return new BaseResponse()
            //     {
            //         IsSuccess = false,
            //         Message = "Trader Cant confirm this transaction"
            //     };
            // }    
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
                    Message = "You're not Allowed To Process As A buyer",
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

        public async Task<BaseResponse> MakeTransactionActive(string transactionId, string email)
        {
           var getTran = await _transactionRepo.GetTransactionByReferenceNumber(transactionId);
              if (getTran==null)
              {
                return new BaseResponse()
                {
                     Message = "Transaction Not Found",
                     IsSuccess = false
                };
              }

              var getTransactionPayment = await _paymentRepo.GetPaymentByReferenceNumber(getTran.ReferenceNumber);
                if (getTransactionPayment==null)
                {
                    return new BaseResponse()
                    {
                        Message = "Transaction Payment Not Found",
                        IsSuccess = false
                    };
                }
                if (getTransactionPayment.Status==PaymentStatus.Success)
                {
                    var getTrader = await _traderRepo.GetTraderByEmailAsync(email);
                    if (getTrader.Email==getTran.BuyerId)
                    {
                        var changeStatus = getTran.Status = TransactionStatus.isActive;
                        var update = await _transactionRepo.UpdateTransaction(getTran);
                        if (update==null)
                        {
                            return new BaseResponse()
                            {
                                Message = "Transaction Not Active",
                                IsSuccess = false
                            };
                        }
                        return new BaseResponse()
                        {
                            Message = "Transaction Active",
                            IsSuccess = true
                        };
                    }
                }
                return new BaseResponse()
                {
                    Message = "Transaction Payment Not Successful",
                    IsSuccess = false
                };
        }

        public async Task<BaseResponse> ReleaseTransactionFunds(string transactionId, string transactionTypeRef, string email)
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
            var getTransactionType = await _transactionTypeRepo.GetTransactionTypeByReference(transactionTypeRef,getTransaction.Id);
            if (getTransactionType==null)
            {
                return new BaseResponse()
                {
                    Message = "Transaction Type Not Found",
                    IsSuccess = false
                };
            }
            var getTrader = await _traderRepo.GetTraderByEmailAsync(email);
            if (getTrader.Email == getTransaction.BuyerId)
            {
                var release = getTransactionType.Status = TransactionTypeEnum.Accpeted;
                var update = await _transactionTypeRepo.UpdateTransactionType(getTransactionType);
                if (update==null)
                {
                    return new BaseResponse()
                    {
                        Message = "Transaction Payment Not Released",
                        IsSuccess = false
                    };
                }
                return new BaseResponse()
                {
                    Message = "Transaction Payment Released",
                    IsSuccess = true
                };
            }
            return new BaseResponse()
            {
                Message = "You're not Allowed To Release Funds",
                IsSuccess = false
            };
        }

        public async Task<BaseResponse> RejectTransaction(string transactionId, string email)
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
            if (getTrader.Email==getTransaction.SellerId)
            {
                if (getTransaction.Status == TransactionStatus.isIntialized)
                {
                    var get = getTransaction.Status = TransactionStatus.IsRejected;
                    var updateTransaction = await _transactionRepo.UpdateTransaction(getTransaction);
                    return new BaseResponse()
                    {
                        IsSuccess = true,
                        Message = "Transaction Rejected Successfully"
                    };
                }
                return new BaseResponse()
                {
                    Message = "You Cant Reject Transaction",
                    IsSuccess = false
                };
            }
            {
                return new BaseResponse()
                {
                    Message = "You're not Allowed To Reject As A buyer",
                    IsSuccess = false
                };
            }  
        }

        public async Task<BaseResponse> CompleteTransaction(string transactionId, string email)
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
            var getTransactionType = await _transactionTypeRepo.GetAllTransactionTypeByReferenceNumber(getTransaction.Id);
            if (getTransactionType==null)
            {
                return new BaseResponse()
                {
                    Message = "Transaction Type Not Found",
                    IsSuccess = false
                };
            }

            foreach (var item in getTransactionType)
            {
                if (item.Status==TransactionTypeEnum.Active)
                {
                    var getTrader = await _traderRepo.GetTraderByEmailAsync(email);
                    if (getTrader.Email==getTransaction.BuyerId)
                    {
                        var get = getTransaction.Status = TransactionStatus.IsCompleted;
                        var updateTransaction = await _transactionRepo.UpdateTransaction(getTransaction);
                        return new BaseResponse()
                        {
                            IsSuccess = true,
                            Message = "Transaction Completed Successfully"
                        };
                    }
                    return new BaseResponse()
                    {
                        Message = "You're not Allowed To Complete Transaction",
                        IsSuccess = false
                    };
                }
                return new BaseResponse(){
                    Message = "Transaction cant be Completed because not all sub has been accepted",
                    IsSuccess = false
                };
            }
            return new BaseResponse()
            {
                Message = "Transaction Completed Successfully",
                IsSuccess = true
            };
            
        }

        public async Task<TransactionListResponseModel> GetInitiatedTransactionByTraderEmail(string email)
        {
            var getTrader = await _traderRepo.GetTraderByEmailAsync(email);
            if (getTrader==null)
            {
                return new TransactionListResponseModel()
                {
                    Message = "Trader Not Found",
                    IsSuccess = false
                };
            }
            var getIntiatedTransaction = await _transactionRepo.GetInitiatedTransactionByTraderEmail(email);
            if (getIntiatedTransaction==null)
            {
                return new TransactionListResponseModel()
                {
                    Message = "No Transaction Found",
                    IsSuccess = false
                };
            }
            return new TransactionListResponseModel()
            {
                TransactionList = getIntiatedTransaction.Select(x => new TransactionDto()
                {
                    reference_id = x.ReferenceNumber,
                    transaction_status = x.Status,
                    BuyerId = x.BuyerId,
                    TotalPrice = x.TotalPrice,
                    DeliveryAddress = x.DeliveryAddress,
                    ItemQuantity = x.ItemQuantity,
                    ItemTitle = x.ItemTitle,
                    SellerId = x.SellerId,
                    CreatedDate = x.CreatedDate,
                }).ToList(),
                Message = "Transactions Found",
                IsSuccess = true
            };
        }

        public async Task<TransactionListResponseModel> GetAgreedTransactionByTraderEmail(string email)
        {
            var getTrader = await _traderRepo.GetTraderByEmailAsync(email);
            if (getTrader==null)
            {
                return new TransactionListResponseModel()
                {
                    Message = "Trader Not Found",
                    IsSuccess = false
                };
            }
            var getAgreedTransaction = await _transactionRepo.GetAgreedTransactionByTraderEmail(email);
            if (getAgreedTransaction==null)
            {
                return new TransactionListResponseModel()
                {
                    Message = "No Transaction Found",
                    IsSuccess = false
                };
            }
            return new TransactionListResponseModel()
            {
                TransactionList = getAgreedTransaction.Select(x => new TransactionDto()
                {
                    reference_id = x.ReferenceNumber,
                    transaction_status = x.Status,
                    BuyerId = x.BuyerId,
                    DeliveryAddress = x.DeliveryAddress,
                    TotalPrice = x.TotalPrice,
                    ItemQuantity = x.ItemQuantity,
                    ItemTitle = x.ItemTitle,
                    SellerId = x.SellerId,
                    CreatedDate = x.CreatedDate,
                }).ToList(),
                Message = "Transactions Found",
                IsSuccess = true
            };
        }

        public async Task<TransactionListResponseModel> GetCompletedTransactionByTraderEmail(string email)
        {
            var getTrader = await _traderRepo.GetTraderByEmailAsync(email);
            if (getTrader==null)
            {
                return new TransactionListResponseModel()
                {
                    Message = "Trader Not Found",
                    IsSuccess = false
                };
            }
            var getCompletedTransaction = await _transactionRepo.GetCompletedTransactionByTraderEmail(email);
            if (getCompletedTransaction==null)
            {
                return new TransactionListResponseModel()
                {
                    Message = "No Transaction Found",
                    IsSuccess = false
                };
            }
            return new TransactionListResponseModel()
            {
                TransactionList = getCompletedTransaction.Select(x => new TransactionDto()
                {
                    reference_id = x.ReferenceNumber,
                    transaction_status = x.Status,
                    BuyerId = x.BuyerId,
                    DeliveryAddress = x.DeliveryAddress,
                    TotalPrice = x.TotalPrice,
                    ItemQuantity = x.ItemQuantity,
                    ItemTitle = x.ItemTitle,
                    SellerId = x.SellerId,
                    CreatedDate = x.CreatedDate,
                }).ToList(),
                Message = "Transactions Found",
                IsSuccess = true
            };
        }

        public async Task<TransactionListResponseModel> GetRejectedTransactionByTraderEmail(string email)
        {
            var getTrader = await _traderRepo.GetTraderByEmailAsync(email);
            if (getTrader==null)
            {
                return new TransactionListResponseModel()
                {
                    Message = "Trader Not Found",
                    IsSuccess = false
                };
            }
            var getRejectedTransaction = await _transactionRepo.GetRejectedTransactionByTraderEmail(email);
            if (getRejectedTransaction==null)
            {
                return new TransactionListResponseModel()
                {
                    Message = "No Transaction Found",
                    IsSuccess = false
                };
            }
            return new TransactionListResponseModel()
            {
                TransactionList = getRejectedTransaction.Select(x => new TransactionDto()
                {
                    reference_id = x.ReferenceNumber,
                    transaction_status = x.Status,
                    BuyerId = x.BuyerId,
                    DeliveryAddress = x.DeliveryAddress,
                    TotalPrice = x.TotalPrice,
                    ItemQuantity = x.ItemQuantity,
                    ItemTitle = x.ItemTitle,
                    SellerId = x.SellerId,
                    CreatedDate = x.CreatedDate,
                }).ToList(),
                Message = "Transactions Found",
                IsSuccess = true
            };
        }

        public  async Task<TransactionListResponseModel> GetActiveTransactionByTraderEmail(string email)
        {
            var getTrader = await _traderRepo.GetTraderByEmailAsync(email);
            if (getTrader==null)
            {
                return new TransactionListResponseModel()
                {
                    Message = "Trader Not Found",
                    IsSuccess = false
                };
            }
            var getRejectedTransaction = await _transactionRepo.GetAllActiveTransactionByTraderEmail(email);
            if (getRejectedTransaction==null)
            {
                return new TransactionListResponseModel()
                {
                    Message = "No Transaction Found",
                    IsSuccess = false
                };
            }
            return new TransactionListResponseModel()
            {
                TransactionList = getRejectedTransaction.Select(x => new TransactionDto()
                {
                    reference_id = x.ReferenceNumber,
                    transaction_status = x.Status,
                    BuyerId = x.BuyerId,
                    DeliveryAddress = x.DeliveryAddress,
                    TotalPrice = x.TotalPrice,
                    ItemQuantity = x.ItemQuantity,
                    ItemTitle = x.ItemTitle,
                    SellerId = x.SellerId,
                    CreatedDate = x.CreatedDate,
                }).ToList(),
                Message = "Transactions Found",
                IsSuccess = true
            };
        }

        public async Task<TransactionListResponseModel> GetCancelledTransactionByTraderEmail(string email)
        {
            var getTrader = await _traderRepo.GetTraderByEmailAsync(email);
            if (getTrader==null)
            {
                return new TransactionListResponseModel()
                {
                    Message = "Trader Not Found",
                    IsSuccess = false
                };
            }
            var getCancelledTransaction = await _transactionRepo.GetCancelledTransactionByTraderEmail(email);
            if (getCancelledTransaction==null)
            {
                return new TransactionListResponseModel()
                {
                    Message = "No Transaction Found",
                    IsSuccess = false
                };
            }
            return new TransactionListResponseModel()
            {
                TransactionList = getCancelledTransaction.Select(x => new TransactionDto()
                {
                    reference_id = x.ReferenceNumber,
                    transaction_status = x.Status,
                    BuyerId = x.BuyerId,
                    DeliveryAddress = x.DeliveryAddress,
                    TotalPrice = x.TotalPrice,
                    ItemQuantity = x.ItemQuantity,
                    ItemTitle = x.ItemTitle,
                    SellerId = x.SellerId,
                    CreatedDate = x.CreatedDate,
                }).ToList(),
                Message = "Transactions Found",
                IsSuccess = true
            };
        }

        public async Task<TransactionListResponseModel> GetAllTransactionByTransactionStatus(TransactionStatus status)
        {
            var getTransaction = await _transactionRepo.GetAllTransactionByTransactionStatus(status);
            if (getTransaction==null)
            {
                return new TransactionListResponseModel()
                {
                    Message = "No Transaction Found",
                    IsSuccess = false
                };
            }
            return new TransactionListResponseModel()
            {
                TransactionList = getTransaction.Select(x => new TransactionDto()
                {
                    reference_id = x.ReferenceNumber,
                    transaction_status = x.Status,
                    BuyerId = x.BuyerId,
                    DeliveryAddress = x.DeliveryAddress,
                    TotalPrice = x.TotalPrice,
                    ItemQuantity = x.ItemQuantity,
                    ItemTitle = x.ItemTitle,
                    SellerId = x.SellerId,
                    CreatedDate = x.CreatedDate,
                }).ToList(),
                Message = "Transactions Found",
                IsSuccess = true
            };
        }
    }
}