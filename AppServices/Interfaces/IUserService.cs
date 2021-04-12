using DTOs.RequestDtos;
using DTOs.ResponseDtos;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace OnboardingApp.Infrastructure.Interfaces
{
    public interface IUserService
    {
        Task<LoginResponse> LoginUser(LoginRequest login);
        Task<IdentityResult> RegisterUser(RegisterRequest register);
        Task<LoginResponse> GetUserByEmailAsync(string email);
        Task ResetPasswordAsync(string email);           
    }
}
