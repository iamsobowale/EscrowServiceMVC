using System;

namespace EscrowService.Models
{
    public class MakeATransferData
    {
        public string reference { get; set; }
        public int integration { get; set; }
        public string domain { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string source{ get; set; }
        public string reason { get; set; }
        public int recipient { get; set; }
        public string status { get; set; }
        public string transfer_code { get; set; }
        public int id { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        
        
       
        //     "transfer_code": "TRF_fiyxvgkh71e717b",
        //     "id": 23070321,
        //     "createdAt": "2020-05-13T14:22:49.687Z",
        //     "updatedAt": "2020-05-13T14:22:49.687Z"
    }
}