using DomainEntities;
using DTOs.RequestDtos;
using DTOs.ResponseDtos;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AppServices.IServices
{
    public interface IUserService
    {
        Task<SignInResult> LoginUser(LoginRequest login);
        Task<IdentityResult> RegisterUser(RegisterRequest register);
        Task<User> GetUserByEmailAsync(string email);
        string GenerateJSONWebToken(string userName);
        Task<string> GeneratePasswordResetTokenAsync(User user);
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);
    }
}