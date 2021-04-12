using AppRepo.Interfaces;
using DomainEntities;
using DTOs.RequestDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AppRepo.Repositories
{
    public class UserRepo : IUserRepo 
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

        #region public Methods

        /// <summary>
        /// This Method Will Create the User by using Identity. 
        /// </summary>
        /// <param name="register"></param>
        /// <returns>Return User</returns>
        public async Task<IdentityResult> RegisterUser(RegisterRequest register)
        {
            try
            {
                var result = await userManager.CreateAsync(new User { UserName = register.Email, Email = register.Email, CreatedDate = DateTime.UtcNow }, register.Password);
                return result;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Login the User after getting valid Login Request
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task<SignInResult> LoginUser(LoginRequest login)
        {
            try
            {
                var result = await signInManager.PasswordSignInAsync(login.Email, login.Password, login.RememberMe, false);
                return result;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// This Method Will Return the User
        /// </summary>
        /// <param name="email"></param>
        /// <returns>User</returns>
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

        /// <summary>
        /// This method will generate the Token for Reset Password
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Token</returns>
        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            try
            {
                string token = await userManager.GeneratePasswordResetTokenAsync(user);
                return token;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method will Gnerate the JSON Web Token
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>Token</returns>
        public string GenerateJSONWebToken(string userName)
        {
            var token = new JwtSecurityToken(
            claims: new Claim[] { new Claim(ClaimTypes.Name, userName) },
            notBefore: new DateTimeOffset(DateTime.Now).DateTime,
            expires: new DateTimeOffset(DateTime.Now.AddHours(6)).DateTime,
            issuer: userName,
            signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// This Method Will Reset the Password
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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

        #endregion
    }
}

