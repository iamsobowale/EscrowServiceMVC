using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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
using TransactionInitialize = EscrowService.Models.TransactionInitialize;

namespace EscrowService.Implementation.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepo _paymentRepo;
        private readonly ITransactionRepo _transactionRepo;
        private readonly IPaymentMethodRepo _paymentMethodRepo;
        private readonly ITraderRepo _traderRepo;
        private readonly ITransactionTypeRepo _transactionType;

        public PaymentService(IPaymentRepo paymentRepo, ITransactionRepo transactionRepo, IPaymentMethodRepo paymentMethodRepo, ITraderRepo traderRepo, ITransactionTypeRepo transactionType)
        {
            _paymentRepo = paymentRepo;
            _transactionRepo = transactionRepo;
            _paymentMethodRepo = paymentMethodRepo;
            _traderRepo = traderRepo;
            _transactionType = transactionType;
        }

        public async Task<PayStackResponse> CreatePayment(string transactionReference, string paymentMethod)
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
                    ReferenceNumber = generateId,
                    Amount = SetAmount(gettransaction.TotalPrice)
                };
                var result = await _paymentRepo.CreatePayment(makePayment);
                if (result == null)
                {
                    return new PayStackResponse()
                    {
                       
                        status = false,
                        message = "Payment Failed",
                    };
                }

                var httpClient = new HttpClient();
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
                var responseObject = JsonConvert.DeserializeObject<PayStackResponse>(responseString);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    
                    if (responseObject.status)
                    {
                        return new PayStackResponse()
                        {
                            status = true,
                            message = responseObject.data.authorization_url,
                            data = new TransactionInitialize()
                            {
                                authorization_url = responseObject.data.authorization_url,
                                reference = makePayment.ReferenceNumber
                            }
                        };
                    }
                   
                }
                else
                {
                    return new PayStackResponse()
                    {
                        status = false,
                        message = responseObject.message
                    };
                }

            }

            {
                return new PayStackResponse()
                {
                    status = false,
                    message = "Cannot make payment, transaction is not agreed"
                };
            }
        }

        public async Task<BaseResponse> VerifyPayment(string transactionReference)
        {
            var getTransactionReference = await _paymentRepo.GetPaymentByTransactionReferenceNumber(transactionReference);
            if (getTransactionReference == null)
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
            var baseUri = getHttpClient.BaseAddress = new Uri("https://api.paystack.co/transaction/verify/" + getTransactionReference.ReferenceNumber);
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
                    var getPayment = await _paymentRepo.GetPaymentByReferenceNumber(getTransactionReference.ReferenceNumber);
                    if (getPayment.Status == PaymentStatus.Pending)
                    {
                        getPayment.Status = PaymentStatus.Success;
                        getPayment.PaymentDate = DateTime.UtcNow;
                        getTransactionReference.Transaction.Status = TransactionStatus.isActive;
                        var updatetransaction = await _transactionRepo.UpdateTransaction(getTransactionReference.Transaction);
                        var result = await _paymentRepo.UpdatePayment(getPayment);
                        if (result==null)
                        {
                            return new BaseResponse
                            {
                                IsSuccess = false,
                                Message = responseObject.message
                            };
                        }
                        return new BaseResponse
                        {
                            IsSuccess = true,
                            Message = responseObject.message
                        };
                    }
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Payment already verified"
                    };
                }
            }
           
            return new BaseResponse
            {
                IsSuccess = false,
                Message = "Verification Failed"
            };
            
        }

        public async Task<VerifyBank> VerifyAccountNumber(string subTransaction)
        {
            var getsubTransaction = await _transactionType.GetTransactionTypeByRefrenceName(subTransaction);
            var getSubTransaction = await _transactionRepo.GetTransaction(getsubTransaction.TransactionId);
            var getSeller = await _traderRepo.GetTraderByEmailAsync(getSubTransaction.SellerId);
            if (getSeller == null)
            {
                return new VerifyBank()
                {
                    status = false,
                    message = "Seller not found"
                };
            }

            var getHttpClient = new HttpClient();
            getHttpClient.DefaultRequestHeaders.Accept.Clear();
            getHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var baseUri = getHttpClient.BaseAddress =
                new Uri($"https://api.paystack.co/bank/resolve?account_number={getSeller.AccountNumber}&bank_code={getSeller.BankName}");
            // "https://api.paystack.co/bank?country=nigeria"
            getHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "sk_test_6483775b59a2152f947af8583a987e98eb5c7af2");
            var response =
                await getHttpClient.GetAsync(baseUri);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<VerifyBank>(responseString);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (!responseObject.status)
                {
                    return new VerifyBank()
                    {
                        status = false,
                        message = responseObject.message
                    };
                }

                var splitName = responseObject.data.account_name;
                var splitNameArray = splitName.Split(' ');
                if (responseObject.data.account_number != getSeller.AccountNumber || splitNameArray[0] !=
                    getSeller.LastName.ToUpper() || splitNameArray[1] != getSeller.FirstName.ToUpper())
                {
                    return new VerifyBank()
                    {
                        status = false,
                        message = "Account number and name does not match"
                    };
                }

                var generate = await GenerateRecipients(responseObject);
                if (!generate.status)
                {
                    return new VerifyBank()
                    {
                        status = false,
                        message = generate.message
                    };
                }

                var makeTransfer = await MakeTransfer(getsubTransaction.Price, generate.data.recipient_code);
                if (!makeTransfer.status)
                {
                    return new VerifyBank()
                    {
                        status = false,
                        message = makeTransfer.message
                    };
                }
                return new VerifyBank()
                {
                    status = true,
                    message = makeTransfer.message,
                    data = new VerifyBankData()
                    {
                        reason = generate.data.reason,
                        reference = generate.data.reference,
                        recipient_code = generate.data.recipient_code,
                        amount = makeTransfer.data.amount,
                        currency = makeTransfer.data.currency,
                        status = makeTransfer.data.status,
                        transfer_code = makeTransfer.data.transfer_code
                    }
                };
            }

            return new VerifyBank()
            {
                status = false,
                message = "Cannot verify account number"
            };
            
        }

        public async Task<GenerateRecipient> GenerateRecipients(VerifyBank verifyBank)
        {
            var getHttpClient = new HttpClient();
            getHttpClient.DefaultRequestHeaders.Accept.Clear();
            getHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var baseUri = getHttpClient.BaseAddress = new Uri($"https://api.paystack.co/transferrecipient");
            getHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "sk_test_6483775b59a2152f947af8583a987e98eb5c7af2");
            var response = await getHttpClient.PostAsJsonAsync(baseUri, new
            {
                type = "nuban",
                name = verifyBank.data.account_name,
                account_number = verifyBank.data.account_number,
                bank_code = verifyBank.data.bank_code,
                currency = "NGN",
            });
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<GenerateRecipient>(responseString);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                if (!responseObject.status)
                {
                    return new GenerateRecipient()
                    {
                        status = false,
                        message = responseObject.message
                    };
                }
                return new GenerateRecipient()
                {
                    status = true,
                    message = "Recipient Generated",
                    data = responseObject.data
                };
            }
            return new GenerateRecipient()
            {
                status = false,
                message = responseObject.message
            };
        }

        public async Task<MakeATransfer> MakeTransfer(decimal amounts, string recipientId)
        {
            var getHttpClient = new HttpClient();
            getHttpClient.DefaultRequestHeaders.Accept.Clear();
            getHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var baseUri = getHttpClient.BaseAddress = new Uri($"https://api.paystack.co/transfer");
            getHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "sk_test_6483775b59a2152f947af8583a987e98eb5c7af2");
            var response = await getHttpClient.PostAsJsonAsync(baseUri, new
            {
                
                recipient = recipientId,
                amount = amounts * 100,
                currency = "NGN",
                source = "balance"
            });
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<MakeATransfer>(responseString);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (!responseObject.status)
                {
                    return new MakeATransfer()
                    {
                        status = false,
                        message = responseObject.message
                    };
                }
                return new MakeATransfer()
                {
                    status = true,
                    message = responseObject.message,
                    data = responseObject.data
                };
            }
            return new MakeATransfer()
            {
                status = false,
                message = responseObject.message
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

        private decimal SetAmount(decimal amount)
        {
            
            if (amount<=5000)
            {
                throw new Exception("Amount must be greater than 5000");
            }
            else if (amount <1000000)
            {
                amount += (amount * 0.005m);
            }
            else if (amount < 1000000)
            {
                amount += (amount * 0.003m);
            }
            else if (amount < 5000000)
            {
                amount += (amount * 0.002m);
            }
            else
            {
                amount += (amount * 0.001m);
            }
            return amount;
        }
    }
}