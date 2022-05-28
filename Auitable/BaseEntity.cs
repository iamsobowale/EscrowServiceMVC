using System;

namespace EscrowService.Auitable
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public bool IsSuspended { get; set; }
        public bool IsDeleted{ get; set; }
        public string AddressLine1 { get; set; }
        public string Document { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string QRCode { get; set; }
        public string QRCodeImage { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}