using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EscrowService.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EscrowService.DTO
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        [EnumDataType(typeof(TransactionStatus))]
        [JsonConverter(typeof(StringEnumConverter))]
        public Role Role { get; set; }
    }

    public class CreateUserRequestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
    }

    public class UpdateUserRequestModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
    }
    public class UserLoginRequest
    {
        public string Email{get; set;}
        public string Password{get; set;}
    }
    public class UserLoginResponse
    {
        public string Token { get; set; }
        public UserDto Data { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
    }
    public class UserResponseModel : BaseResponse
    {
        public UserDto Data{get;set;}
    }
    public class UsersResponseModel : BaseResponse
    {
        public List<UserDto> Data{get;set;}
    }
    
}

