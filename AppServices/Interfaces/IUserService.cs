using AppServices.EmailService;
using DomainEntities;
using DTOs.RequestDtos;
using DTOs.ResponseDtos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IUserService
    {
        Task<LoginResponse> LoginUser(LoginRequest login);
        Task<IdentityResult> RegisterUser(RegisterRequest register);
        Task<LoginResponse> GetUserByEmailAsync(string email);
        Task ResetPasswordAsync(string email);           
    }
}
