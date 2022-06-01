using System;
using System.Linq;
using System.Threading.Tasks;
using EscrowService.DTO;
using EscrowService.Interface.Repository;
using EscrowService.Interface.Service;
using EscrowService.Models;

namespace EscrowService.Implementation.Service
{
    public class AdminService:IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IUserRepo _userRepo;

        public AdminService(IAdminRepository adminRepository, IUserRepo userRepo)
        {
            _adminRepository = adminRepository;
            _userRepo = userRepo;
        }

        public async Task<BaseResponse> CreateAdminAsync(CreateAdminRequestModel requestModel)
        {
            var genertateId = $"AdminId{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5).ToUpper()}";
            var user = new User
            {
                Email = requestModel.Email,
                Password = requestModel.Password,
                Role = Role.Admin,
            };
            var createUser = await _userRepo.CreateUser(user);
            var admin = new Admin
            {
                UserId = user.Id,
                User = user,
                FirstName = requestModel.FirstName,
                LastName = requestModel.LastName,
                PhoneNumber = requestModel.PhoneNumber,
                City = requestModel.City,
                State = requestModel.State,
                CreatedDate = DateTime.Now,
                Email = requestModel.Email,
                Password = requestModel.Password,
                Role = Role.Admin,
                Gender = requestModel.Gender,
                Dob = requestModel.Dob,
                AdminId = genertateId
            };
            var createAdmin =await _adminRepository.CreateAdminAsync(admin);
            if (createAdmin==null)
            {
                return new BaseResponse()
                {
                    IsSuccess = false, Message = "Admin not created"
                };
            }
            
            return new BaseResponse()
            {
                IsSuccess = true, Message = "Admin created"
            };
        }

        public async Task<BaseResponse> UpdateAdminAsync(UpdateAdminRequestModel requestModel, int id)
        {
            var admin = await _adminRepository.GetAdminAsync(id);
            if (admin == null)
            {
                return new BaseResponse()
                {
                    IsSuccess = false, Message = "Admin not found"
                };
            }
            var getuser = await _userRepo.GetUser(admin.UserId);
            if (getuser == null)
            {
                return new BaseResponse()
                {
                    IsSuccess = false, Message = "User not found"
                };
            }
            getuser.Email = requestModel.Email;
            getuser.Password = requestModel.Password;
            var updateUser = await _userRepo.UpdateUser(getuser);
            admin.FirstName = requestModel.FirstName;
            admin.LastName = requestModel.LastName;
            admin.PhoneNumber = requestModel.PhoneNumber;
            admin.City = requestModel.City;
            admin.State = requestModel.State;
            admin.Email = requestModel.Email;
            admin.Password = requestModel.Password;
            admin.Dob = requestModel.Dob;
            var updateAdmin = await _adminRepository.UpdateAdminAsync(admin);
            if (updateAdmin == null)
            {
                return new BaseResponse()
                {
                    IsSuccess = false, Message = "Admin not updated"
                };
            }
            return new BaseResponse()
            {
                IsSuccess = true, Message = "Admin updated"
            };
        }

        public async Task<bool> DeleteAdminAsync(int id)
        {
           var getAdmin =await _adminRepository.GetAdminAsync(id);
           if (getAdmin == null)
           {
               return false;
           }
           getAdmin.IsDeleted = true;
           var deleteAdmin = await _adminRepository.UpdateAdminAsync(getAdmin);
            if (deleteAdmin == null)
            {
                return false;
            }
            return true;
        }

        public async Task<AdminResponseModel> GetAdminAsync(int id)
        {
            var admin = await _adminRepository.GetAdminAsync(id);
            if (admin == null)
            {
                return new AdminResponseModel()
                {
                    IsSuccess = false, 
                    Message = "Admin not found"
                };
            }

            return new AdminResponseModel()
            {
                Admin = new AdminDto()
                {
                    Id = admin.Id,
                    FirstName = admin.FirstName,
                    LastName = admin.LastName,
                    PhoneNumber = admin.PhoneNumber,
                    City = admin.City,
                    State = admin.State,
                    Email = admin.Email,
                    Dob = admin.Dob,
                    AdminId = admin.AdminId
                },
                IsSuccess = true,
                Message = "Admin found"
            };
        }

        public async Task<AdminResponsesModel> GetAllAdminsAsync()
        {
           var getAdmins = await _adminRepository.GetAllAdminAsync();
            return new AdminResponsesModel()
            {
                IsSuccess = true,
                Message = "Admins found",
                Admin = getAdmins.Select(s => new AdminDto()
                {
                    FirstName = s.FirstName,
                    AdminId = s.AdminId,
                    LastName = s.LastName,
                    PhoneNumber = s.PhoneNumber,
                    City = s.City,
                    State = s.State,
                    Email = s.Email,
                    Dob = s.Dob,
                }).ToList()
            };
        }

        public async Task<AdminResponseModel> GetAdminByEmailAsync(string email)
        {
           var getAdmin = await _adminRepository.GetAdminByEmailAsync(email);
           if (getAdmin == null)
           {
               return new AdminResponseModel()
               {
                   IsSuccess = false,
                   Message = "Admin not found"
               };
           }
           return new AdminResponseModel()
           {
               Admin = new AdminDto()
               {
                   Id = getAdmin.Id,
                   FirstName = getAdmin.FirstName,
                   LastName = getAdmin.LastName,
                   PhoneNumber = getAdmin.PhoneNumber,
                   City = getAdmin.City,
                   State = getAdmin.State,
                   Email = getAdmin.Email,
                   Dob = getAdmin.Dob,
                   AdminId = getAdmin.AdminId
               },
               IsSuccess = true,
               Message = "Admin found"
           };
        }

        public async Task<AdminResponseModel> GetAdminByUserIdAsync(int userId)
        {
            var getAdmin = await _adminRepository.GetAdminByUserIdAsync(userId);
            if (getAdmin == null)
            {
                return new AdminResponseModel()
                {
                    IsSuccess = false,
                    Message = "Admin not found"
                };
            }
            return new AdminResponseModel()
            {
                Admin = new AdminDto()
                {
                    Id = getAdmin.Id,
                    FirstName = getAdmin.FirstName,
                    LastName = getAdmin.LastName,
                    PhoneNumber = getAdmin.PhoneNumber,
                    City = getAdmin.City,
                    State = getAdmin.State,
                    Email = getAdmin.Email,
                    Dob = getAdmin.Dob,
                    AdminId = getAdmin.AdminId
                },
                IsSuccess = true,
                Message = "Admin found"
            };
        }
    }
}