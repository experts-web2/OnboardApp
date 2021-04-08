using DomainEntities;
using DTOs.RequestDtos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppRepo.Interfaces
{
    public interface IUserRepo
    {
        Task<SignInResult> LoginUser(LoginRequest login);
        Task<IdentityResult> RegisterUser(RegisterRequest register);
        Task<User> GetUserByEmailAsync(string email);
        string GenerateJSONWebToken(string userName);
        Task<string> GeneratePasswordResetTokenAsync(User user);
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);
    }
}
