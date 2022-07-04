using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EscrowService.DTO;
using EscrowService.Interface.Repository;
using EscrowService.Interface.Service;
using EscrowService.Models;
using Org.BouncyCastle.Crypto.Generators;

namespace EscrowService.Implementation.Service
{
    public class TraderService:ITraderService
    {
        public readonly ITraderRepo _traderRepo;
        private readonly IUserRepo _userRepo;

        public TraderService(ITraderRepo traderRepo, IUserRepo userRepo)
        {
            _traderRepo = traderRepo;
            _userRepo = userRepo;
        }

        public async Task<BaseResponse> CreateTraderAsync(CreateTraderRequestModel requestModel)
        {
            var checkEmail = await _userRepo.EmailExistsAsync(requestModel.Email);
            if (checkEmail)
            {
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Email already exists"
                };
            }
            var user= new User()
            {
                Password = requestModel.Password,
                Email = requestModel.Email,
                Role = Role.Trader
            };
            var response = await _userRepo.CreateUser(user);
            if (response==null)
            {
                return new BaseResponse()
                {
                    Message = "Failed",
                    IsSuccess = false
                };
            }
            
            var trader = new Trader()
            {
                UserId = user.Id,
                User = user,
                Phone = requestModel.PhoneNumber,
                City = requestModel.City,
                Email = requestModel.Email,
                AddressLine1 = requestModel.Address,
                State = requestModel.State,
                FirstName = requestModel.FirstName,
                Dob = requestModel.Dob,
                AccountName = requestModel.AccountName,
                AccountNumber = requestModel.AccountNumber,
                BankName = requestModel.BankName,
                LastName = requestModel.LastName,
                Gender = requestModel.Gender
            };
            var  create = await _traderRepo.CreateTraderAsync(trader);
            if (create==null)
            {
                return new BaseResponse()
                {
                    IsSuccess = false,
                    Message = "Trader Not Created"
                };
            }
            return new BaseResponse()
            {
                IsSuccess = true,
                Message = "Trader Created Successfully"
            };
        }
        public async Task<BaseResponse> UpdateTraderAsync(TraderUpdateRequestModel requestModel, string id)
        {
            var trader = await _traderRepo.GetTraderByEmailAsync(id);
            if (trader==null)
            {
                return new BaseResponse()
                {
                    IsSuccess = false,
                    Message = "Trader Not Found"
                };
            }
            var user = await _userRepo.GetUser(trader.UserId);
            if (user==null)
            {
                return new BaseResponse()
                {
                    IsSuccess = false,
                    Message = "User Not Found"
                };
            }
            user.Email = requestModel.Email;
            user.Password = requestModel.Password;
            await _userRepo.UpdateUser(user);
            trader.FirstName = requestModel.FirstName;
            trader.LastName = requestModel.LastName;
            trader.Phone = requestModel.Phone;
            trader.City = requestModel.City;
            trader.Email = requestModel.Email;
            trader.AddressLine1 = requestModel.Address;
            trader.State = requestModel.State;
            trader.AccountName = requestModel.AccountName;
            trader.AccountNumber = requestModel.AccountNumber;
            trader.BankName = requestModel.BankName;
            var update = await _traderRepo.UpdateTraderAsync(trader);
            if (update==null)
            {
                return new BaseResponse()
                {
                    IsSuccess = false,
                    Message = "Trader Not Updated"
                };
            }
            return new BaseResponse()
            {
                IsSuccess = true,
                Message = "Trader Updated Successfully"
            };
        }

        public async Task<bool> DeleteTraderAsync(int id)
        {
            var getTrader = await _traderRepo.GetTraderAsync(id);
            if (getTrader==null)
            {
                return false;
            }
            var getUser = await _userRepo.GetUser(getTrader.UserId);
            if (getUser==null)
            {
                return false;
            }
            getTrader.IsDeleted = true;
            getUser.IsDeleted = true;
            var delete = await _traderRepo.UpdateTraderAsync(getTrader);
            return true;
        }

        public async Task<TradersResponseModel> GetTraderAsync(int id)
        {
           var trader = await _traderRepo.GetTraderAsync(id);
            if (trader==null)
            {
                return new TradersResponseModel()
                {
                    IsSuccess = false,
                    Message = "Trader Not Found"
                };
            }
            return new TradersResponseModel()
            {
                Traders = new TradersDto()
                {
                    FirstName = trader.FirstName,
                    LastName = trader.LastName,
                    PhoneNumber = trader.Phone,
                    City = trader.City,
                    Email = trader.Email,
                    Address = trader.AddressLine1,
                    State = trader.State,
                    AccountName = trader.AccountName,
                    AccountNumber = trader.AccountNumber,
                    BankName = trader.BankName,
                    Dob = trader.Dob,
                    Gender = trader.Gender
                },
                Message = "Success",
                IsSuccess = true
            };
        }

        public async Task<TraderResponsesModel> GetAllTradersAsync()
        {
           var getall = await _traderRepo.GetAllTraderAsync(d => d.IsDeleted == false);
           return new TraderResponsesModel()
           {
               Message = "Found",
               IsSuccess = true,
               Traders = getall.Select(y => new TradersDto
               {
                   Gender = y.Gender,
                   FirstName = y.FirstName,
                   LastName = y.LastName,
                   PhoneNumber = y.Phone,
                   City = y.City,
                   Email = y.Email,
                   Address = y.AddressLine1,
                   State = y.State,
                   AccountName = y.AccountName,
                   AccountNumber = y.AccountNumber,
                   BankName = y.BankName,
                   Dob = y.Dob,
               }).ToList()
           };
        }

        public async Task<TradersResponseModel> GetTraderByEmailAsync(string email)
        {
            var getbyEmail = await _traderRepo.GetTraderByEmailAsync(email);
            if (getbyEmail==null)
            {
                return new TradersResponseModel()
                {
                    IsSuccess = false,
                    Message = "Trader Not Found"
                };
            }
            return new TradersResponseModel()
            {
                Traders = new TradersDto()
                {
                    FirstName = getbyEmail.FirstName,
                    LastName = getbyEmail.LastName,
                    PhoneNumber = getbyEmail.Phone,
                    City = getbyEmail.City,
                    Email = getbyEmail.Email,
                    Address = getbyEmail.AddressLine1,
                    State = getbyEmail.State,
                    AccountName = getbyEmail.AccountName,
                    AccountNumber = getbyEmail.AccountNumber,
                    BankName = getbyEmail.BankName,
                    Dob = getbyEmail.Dob,
                    Gender = getbyEmail.Gender
                },
                Message = "Success",
                IsSuccess = true
            };
        }

        public async Task<TraderResponsesModel> GetAllTradersInTransaction(string referenceNumber)
        {
            var getAllTradersInTransaction = await _traderRepo.GetAllTradersInTransaction(referenceNumber);
            return new TraderResponsesModel()
            {
                Traders = getAllTradersInTransaction.Select(y => new TradersDto()
                {
                    Gender = y.Trader.Gender,
                    FirstName = y.Trader.FirstName,
                    LastName = y.Trader.LastName,
                    PhoneNumber = y.Trader.Phone,
                    City = y.Trader.City,
                    Email = y.Trader.Email,
                    Address = y.Trader.AddressLine1,
                    State = y.Trader.State,
                    AccountName = y.Trader.AccountName,
                    AccountNumber = y.Trader.AccountNumber,
                    BankName = y.Trader.BankName,
                    Dob = y.Trader.Dob,
                }).ToList(),
                Message = "Success",
                IsSuccess = true
            };
        }

        public Task<TradersResponseModel> GetTraderByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }
        
    }
}