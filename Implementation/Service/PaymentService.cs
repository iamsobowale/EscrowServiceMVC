using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EscrowService.DTO;
using EscrowService.Interface.Repository;
using EscrowService.Interface.Service;
using EscrowService.Models;
using Newtonsoft.Json;
using PayStack.Net;

namespace EscrowService.Implementation.Service
{
    public class PaymentService:IPaymentService
    {
        private readonly IPaymentRepo _paymentRepo;
        private readonly ITransactionRepo _transactionRepo;
        private readonly IPaymentMethodRepo _paymentMethodRepo;
        public PaymentService(IPaymentRepo paymentRepo, ITransactionRepo transactionRepo, IPaymentMethodRepo paymentMethodRepo)
        {
            _paymentRepo = paymentRepo;
            _transactionRepo = transactionRepo;
            _paymentMethodRepo = paymentMethodRepo;
        }

        public async Task<BaseResponse> CreatePayment(string transactionReference, string paymentMethod)
        {
            var generateId = $"AdminId{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5).ToUpper()}";
            var gettransaction = await _transactionRepo.GetTransactionByReferenceNumber(transactionReference);
            if (gettransaction.Status == TransactionStatus.isAgreed)
            {
                var getpaymentmethod = await _paymentMethodRepo.GetPaymentMethodByName(paymentMethod);
                var makePayment = new Payment
                {
                    TransactionId = gettransaction.Id,
                    PaymentMethodId = getpaymentmethod.Id,
                    PaymentDate = DateTime.UtcNow,
                    Status = PaymentStatus.Pending,
                    ReferenceNumber = generateId,
                    Amount = SetAmount(gettransaction.TotalPrice)
                };
                var result = await _paymentRepo.CreatePayment(makePayment);
                if (result==null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Payment Failed"
                    };
                }
                makePayment.Status = PaymentStatus.Success;
                var updatepayment = await _paymentRepo.UpdatePayment(makePayment);
                if (updatepayment == null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Payment Failed"
                    };
                }
                gettransaction.Status = TransactionStatus.isActive;
                await _transactionRepo.UpdateTransaction(gettransaction);
                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Payment Success"
                };
            }
            {
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Cannot make payment, transaction is not agreed"
                };
            }
        }

        public async Task<PaymentResponseDto> GetPayment(int paymentId)
        {
           var getPayment = await _paymentRepo.GetPayment(paymentId);
            if (getPayment == null)
            {
                return new PaymentResponseDto
                {
                    IsSuccess = false,
                    Message = "Payment not found"
                };
            }
            return new PaymentResponseDto
            {
                IsSuccess = true,
                Payment = new PaymentDto()
                {
                    Amount = getPayment.Amount,
                    PaymentDate = getPayment.PaymentDate,
                    PaymentMethodName = getPayment.PaymentMethod.Name,
                    ReferenceNumber = getPayment.ReferenceNumber,
                    TransactionReferenceNumber = getPayment.Transaction.ReferenceNumber,
                    PaymentStatus = getPayment.Status.ToString(),
                }
            };
        }

        public async Task<PaymentResponseDto> GetPaymentByReferenceNumber(string referenceNumber)
        {
           var get = await _paymentRepo.GetPaymentByReferenceNumber(referenceNumber);
            if (get == null)
            {
                return new PaymentResponseDto
                {
                    IsSuccess = false,
                    Message = "Payment not found"
                };
            }
            return new PaymentResponseDto
            {
                IsSuccess = true,
                Payment = new PaymentDto()
                {
                    Amount = get.Amount,
                    PaymentDate = get.PaymentDate,
                    PaymentMethodName = get.PaymentMethod.Name,
                    ReferenceNumber = get.ReferenceNumber,
                    TransactionReferenceNumber = get.Transaction.ReferenceNumber,
                    PaymentStatus = get.Status.ToString(),
                }
            };
        }

        public async Task<PaymentListResponseDto> GetPaymentByPaymentStatus(PaymentStatus paymentStatus)
        {
            var get = await _paymentRepo.GetPaymentByPaymentStatus(paymentStatus);
            if (get == null)
            {
                return new PaymentListResponseDto
                {
                    IsSuccess = false,
                    Message = "Payment not found"
                };
            }
            return new PaymentListResponseDto
            {
                IsSuccess = true,
                Payments = get.Select(x => new PaymentDto()
                {
                    Amount = x.Amount,
                    PaymentDate = x.PaymentDate,
                    PaymentMethodName = x.PaymentMethod.Name,
                    ReferenceNumber = x.ReferenceNumber,
                    TransactionReferenceNumber = x.Transaction.ReferenceNumber,
                    PaymentStatus = x.Status.ToString(),
                }).ToList()
            };
        }

        public async Task<PaymentListResponseDto> GetSuccessfulPaymentByPaymentStatus(string transactionReference)
        {
            var get = await _paymentRepo.GetAllSuccessfulPaymentByStatus(transactionReference);
            if (get == null)
            {
                return new PaymentListResponseDto
                {
                    IsSuccess = false,
                    Message = "Payment not found"
                };
            }
            return new PaymentListResponseDto
            {
                IsSuccess = true,
                Payments = get.Select(x => new PaymentDto()
                {
                    Amount = x.Amount,
                    PaymentDate = x.PaymentDate,
                    PaymentMethodName = x.PaymentMethod.Name,
                    ReferenceNumber = x.ReferenceNumber,
                    TransactionReferenceNumber = x.Transaction.ReferenceNumber,
                    PaymentStatus = x.Status.ToString(),
                }).ToList()
            };
        }

        public async Task<PaymentListResponseDto> GetPendingPaymentByPaymentStatus(string transactionReference)
        {
            var get = await _paymentRepo.GetAllPendingPaymentByStatus(transactionReference);
            if (get == null)
            {
                return new PaymentListResponseDto
                {
                    IsSuccess = false,
                    Message = "Payment not found"
                };
            }
            return new PaymentListResponseDto
            {
                IsSuccess = true,
                Payments = get.Select(x => new PaymentDto()
                {
                    Amount = x.Amount,
                    PaymentDate = x.PaymentDate,
                    PaymentMethodName = x.PaymentMethod.Name,
                    ReferenceNumber = x.ReferenceNumber,
                    TransactionReferenceNumber = x.Transaction.ReferenceNumber,
                    PaymentStatus = x.Status.ToString(),
                }).ToList()
            };
        }

        public Task<PaymentListResponseDto> GetPaymentByPaymentMethod(string paymentMethod)
        {
            throw new System.NotImplementedException();
        }

        // public async Task<string> MakePaymentWithPaystack(string clientid, string secretkey)
        // {
        //     var request = new HttpRequestMessage(HttpMethod.Post, "token");
        //     request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientid}:{secretkey}")));
        //     request.Content = new FormUrlEncodedContent((new Dictionary<string, string>
        //     {
        //         { "grant_type", "client_credentials" }
        //     }));
        //     var response = await _httpClient.SendAsync(request);
        //     response.EnsureSuccessStatusCode();
        //     var responseBody = await response.Content.ReadAsStreamAsync();
        //     var authResponse = await JsonSerializer.Create().Deserialize<>(responseBody);
        //
        // }

        private decimal SetAmount(decimal amount)
        {
            
            if (amount<=5000)
            {
                throw new Exception("Amount must be greater than 5000");
            }
            else if (amount <=500000)
            {
                amount += (amount * 0.005m);
            }
            else if (amount >= 1000000)
            {
                amount += (amount * 0.003m);
            }
            else if (amount >= 500000)
            {
                amount += (amount * 0.002m);
            }
            return amount;
        }
    }
}