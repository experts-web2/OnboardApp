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
        Task<User> GetUserByEmailAsync(string email);
        string GenerateJSONWebToken(string userName);
        Task<string> GeneratePasswordResetTokenAsync(User user);
    }
}