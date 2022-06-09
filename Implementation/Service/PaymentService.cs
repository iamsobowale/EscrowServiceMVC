using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using EscrowService.DTO;
using EscrowService.Interface.Repository;
using EscrowService.Interface.Service;
using EscrowService.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PayStack.Net;

namespace EscrowService.Implementation.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepo _paymentRepo;
        private readonly ITransactionRepo _transactionRepo;
        private readonly IPaymentMethodRepo _paymentMethodRepo;

        public PaymentService(IPaymentRepo paymentRepo, ITransactionRepo transactionRepo,
            IPaymentMethodRepo paymentMethodRepo)
        {
            _paymentRepo = paymentRepo;
            _transactionRepo = transactionRepo;
            _paymentMethodRepo = paymentMethodRepo;
        }

        public async Task<BaseResponse> CreatePayment(string transactionReference, string paymentMethod)
        {
            var generateId = $"PaymentId{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5).ToUpper()}";
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
                    ReferenceNumber = gettransaction.ReferenceNumber,
                    Amount = SetAmount(gettransaction.TotalPrice)
                };
                var result = await _paymentRepo.CreatePayment(makePayment);
                if (result == null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Payment Failed"
                    };
                }

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.BaseAddress = new Uri("https://api.paystack.co/transaction/initialize");
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", "sk_test_6483775b59a2152f947af8583a987e98eb5c7af2");
                var content = new StringContent(JsonConvert.SerializeObject(new
                {
                    amount = makePayment.Amount * 100,
                    email = gettransaction.BuyerId,
                    reference = makePayment.ReferenceNumber,
                    metadata = new
                    {
                        transaction_id = makePayment.TransactionId,
                        payment_method = makePayment.PaymentMethodId
                    }
                }), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://api.paystack.co/transaction/initialize", content);
                var responseString = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var responseObject = JsonConvert.DeserializeObject<PayStackResponse>(responseString);

                    if (responseObject.status)
                    {

                        return new BaseResponse
                        {
                            IsSuccess = true,
                            Message = responseObject.data.authorization_url
                        };
                    }
                    else
                    {
                        return new BaseResponse
                        {
                            IsSuccess = false,
                            Message = responseObject.message
                        };
                    }
                }
                else
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Payment Failed"
                    };
                }

            }

            {
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Cannot make payment, transaction is not agreed"
                };
            }
        }

        public async Task<BaseResponse> VerifyPayment(string TransactionRefernce)
        {
            var getTransactionRefernce = await _paymentRepo.GetPaymentByReferenceNumber(TransactionRefernce);
            if (getTransactionRefernce == null)
            {
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Transaction not found"
                };
            }

            var getHttpClient = new HttpClient();
            getHttpClient.DefaultRequestHeaders.Accept.Clear();
            getHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var baseUri = getHttpClient.BaseAddress = new Uri("https://api.paystack.co/transaction/verify/" + TransactionRefernce);
            getHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "sk_test_6483775b59a2152f947af8583a987e98eb5c7af2");
            var response =
                await getHttpClient.GetAsync(baseUri);
            var responseString = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseObject = JsonConvert.DeserializeObject<PayStackResponse>(responseString);
                if (responseObject.status)
                {
                    var getPayment = await _paymentRepo.GetPaymentByReferenceNumber(getTransactionRefernce.ReferenceNumber);
                    if (getPayment.Status == PaymentStatus.Pending)
                    {
                        getPayment.Status = PaymentStatus.Success;
                        getPayment.PaymentDate = DateTime.UtcNow;
                        var result = await _paymentRepo.UpdatePayment(getPayment);
                        if (result==null)
                        {
                            return new BaseResponse
                            {
                                IsSuccess = false,
                                Message = "Payment Failed"
                            };
                        }
                        return new BaseResponse
                        {
                            IsSuccess = true,
                            Message = "Payment Already Verified"
                        };
                    }
                    else
                    {
                        return new BaseResponse
                        {
                            IsSuccess = false,
                            Message = "Verification Failed"
                        };
                    }
                }
            }
           
            return new BaseResponse
            {
                IsSuccess = false,
                Message = "Payment Failed"
            };
            
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