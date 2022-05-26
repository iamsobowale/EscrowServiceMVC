using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EscrowService.DTO;
using EscrowService.Interface.Repository;
using EscrowService.Interface.Service;
using EscrowService.Models;

namespace EscrowService.Implementation.Service
{
    public class PaymentMethodService:IPaymentMethodService
    {
        private readonly IPaymentMethodRepo _paymentMethodRepo;


        public PaymentMethodService(IPaymentMethodRepo paymentMethodRepo)
        {
            _paymentMethodRepo = paymentMethodRepo;
        }

        public async Task<BaseResponse> CreatePaymentMethod(CreatePaymentMethodRequestModel _request)
        {
            var createPaymentMethod = new PaymentMethod
            {
               Name = _request.PaymentMethodName,
               Description = _request.PaymentMethodDescription,
               CreatedDate = new DateTime(),
               IsDeleted = false,
            };
            var create = await _paymentMethodRepo.CreatePaymentMethod(createPaymentMethod);
            if (create==null)
            {
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Payment Method not created"
                };
            }
            return new BaseResponse
            {
                IsSuccess = true,
                Message = "Payment Method created successfully"
            };
        }

        public async Task<BaseResponse> UpdatePaymentMethod(UpdatePaymentMethodRequestModel _request, int id)
        {
            var getPayment = await _paymentMethodRepo.GetPaymentMethod(id);
            getPayment.Name = _request.PaymentMethodName;
            getPayment.Description = _request.PaymentMethodDescription;
            getPayment.UpdatedDate = new DateTime();
            var update = await _paymentMethodRepo.UpdatePaymentMethod(getPayment);
            if (update==null)
            {
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Payment Method not updated"
                };
            }
            return new BaseResponse
            {
                IsSuccess = true,
                Message = "Payment Method updated successfully"
            };
            
        }

        public async Task<bool> DeletePaymentMethod(int id)
        {
            var getpayment = await _paymentMethodRepo.GetPaymentMethod(id);
            if (getpayment==null)
            {
                return false;
            }
            getpayment.IsDeleted = true;
            _paymentMethodRepo.UpdatePaymentMethod(getpayment);
            return true;
        }

        public async Task<PaymentMethodResponseModel> GetPaymentMethod(int id)
        {
           var getpayment = await _paymentMethodRepo.GetPaymentMethod(id);
           if (getpayment==null)
           {
               return new PaymentMethodResponseModel()
               {
                   Message = "PaymentMethod Not Found",
                   IsSuccess = false
               };
           }
           return new PaymentMethodResponseModel
            {
                PaymentMethod = new PaymentMethodDto()
                {
                    PaymentMethodName = getpayment.Name,
                    PaymentMethodDescription = getpayment.Description
                }
            };
        }

        public async Task<PaymentMethodResponsesModel> GetAllPaymentMethod()
        {
            var getall = await _paymentMethodRepo.GetAllPaymentMethod();
            return new PaymentMethodResponsesModel()
            {
                Message = "Found",
                IsSuccess = true,
                PaymentMethods = getall.Select(d => new PaymentMethodDto
                {
                    PaymentMethodName = d.Name,
                    PaymentMethodDescription = d.Description
                }).ToList()
            };
        }

        public async Task<PaymentMethodResponseModel> GetPaymentMethodByName(string name)
        {
            var getpayment = await _paymentMethodRepo.GetPaymentMethodByName(name);
            if (getpayment==null)
            {
                return new PaymentMethodResponseModel()
                {
                    Message = "PaymentMethod Not Found",
                    IsSuccess = false
                };
            }
            return new PaymentMethodResponseModel
            {
                PaymentMethod = new PaymentMethodDto()
                {
                    PaymentMethodName = getpayment.Name,
                    PaymentMethodDescription = getpayment.Description
                }
            };
        }
    }
}