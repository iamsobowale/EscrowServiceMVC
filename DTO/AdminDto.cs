using System;
using System.Collections.Generic;

namespace EscrowService.DTO
{
    public class AdminDto
    {
        public int Id { get; set; }
        public string AdminId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public DateTime Dob { get; set; }
    }
    public class CreateAdminRequestModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Gender { get; set; }
        public string State { get; set; }
        public DateTime Dob { get; set; }
        public string Password { get; set; }
    }
    public class UpdateAdminRequestModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public DateTime Dob { get; set; }
        public string Password { get; set; }
    }
    public class AdminResponseModel:BaseResponse
    {
        public AdminDto Admin { get; set; }
    }
    public class AdminResponsesModel:BaseResponse
    {
        public IList<AdminDto> Admin { get; set; }
    }
 
}