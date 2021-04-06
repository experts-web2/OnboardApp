using AppServices.IServices;
using DomainEntities;
using DTOs.RequestDtos;
using DTOs.ResponseDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AppServices.Repositories
{
    public class UserRepo : IUserService
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly SymmetricSecurityKey securityKey;
        private readonly SigningCredentials credentials;

        public UserRepo(IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            
            // Getting Security Key and Credential

            securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecretKey"]));
            credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }
        public async Task<SignInResult> LoginUser(LoginRequest login)
        {
            try
            {
               // await userManager.CreateAsync(new User { UserName = login.Email, Email = login.Email, CreatedDate = DateTime.UtcNow }, login.Password);
                return await signInManager.PasswordSignInAsync(login.Email, login.Password, login.RememberMe, false);
            }
            catch (Exception)
            {
                throw;
            }
              
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                return await userManager.FindByEmailAsync(email);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            try
            {
                string token = await userManager.GeneratePasswordResetTokenAsync(user);
                return token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GenerateJSONWebToken(string userName)
        {           
            var token = new JwtSecurityToken(
            claims: new Claim[] {new Claim(ClaimTypes.Name, userName) },
            notBefore: new DateTimeOffset(DateTime.Now).DateTime,
            expires: new DateTimeOffset(DateTime.Now.AddHours(6)).DateTime,
            issuer: userName,
            signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            try
            {
                return userManager.ResetPasswordAsync(user, token, password);
            }
            catch (Exception)
            {

                throw;
            }
          
        }

    }
}
