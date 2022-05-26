using System.Collections.Generic;

namespace EscrowService.DTO
{
    public class PaymentMethodDto
    {
        public string PaymentMethodName { get; set; }
        public string PaymentMethodDescription { get; set; }
    }
    public class CreatePaymentMethodRequestModel
    {
        public string PaymentMethodName { get; set; }
        public string PaymentMethodDescription { get; set; }
        public int PaymentMethodId { get; set; }
    }
    public class UpdatePaymentMethodRequestModel
    {
        public string PaymentMethodName { get; set; }
        public string PaymentMethodDescription { get; set; }
    }
    
    public class PaymentMethodResponseModel:BaseResponse
    {
        public PaymentMethodDto PaymentMethod { get; set; }
    }
    public class PaymentMethodResponsesModel:BaseResponse
    {
        public IEnumerable<PaymentMethodDto> PaymentMethods { get; set; }
    }
}