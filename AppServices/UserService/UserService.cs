using AppRepo.Interfaces;
using DomainEntities;
using DTOs.RequestDtos;
using DTOs.ResponseDtos;
using Microsoft.AspNetCore.Identity;
using OnboardingApp.Infrastructure.EmailService;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingApp.Infrastructure.UserService
{
    public class UserService : Interfaces.IUserService
    {
        private readonly IUserRepo userRepo;
        private readonly Random random = new Random();
        private readonly IEmailSender _emailSender;
        public UserService(IUserRepo userRepo, IEmailSender _emailSender)
        {
            this.userRepo = userRepo;
            this._emailSender = _emailSender;
        }

        #region public methods

        /// <summary>
        /// This Method Will Givent the user by passing email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Login Response</returns>
        public async Task<LoginResponse> GetUserByEmailAsync(string email)
        {
            User user = await userRepo.GetUserByEmailAsync(email);
            if(user != null)
            {
                LoginResponse loginResponse = new LoginResponse()
                {
                    UserId = user.Id,
                    Token = userRepo.GenerateJSONWebToken(user.UserName),
                    Email = user.Email,
                    CreatedDate = user.CreatedDate,
                };
                return loginResponse;
            }
            else
            {
                throw new Exception("User Not Found");
            }
        }

        /// <summary>
        /// This Method will send the Password to the User Email Address 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task ResetPasswordAsync(string email)
        {
            User user = await userRepo.GetUserByEmailAsync(email);
            if (user != null)
            {
                string token = await userRepo.GeneratePasswordResetTokenAsync(user);
                string password = this.RandomString(8);
                await userRepo.ResetPasswordAsync(user, token, password);

                var message = new Message(new string[] { user.Email }, "New password", "Your New Password is: " + password, null);
                await _emailSender.SendEmailAsync(message);
            }
            else
            {
                throw new Exception("Email do not exist");
            }
        }

        /// <summary>
        /// This Method will Login the user by calling to User Repository 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task<LoginResponse> LoginUser(LoginRequest login)
        {
            var result= await userRepo.LoginUser(login);

            if (result.Succeeded)
            {
                return await GetUserByEmailAsync(login.Email);
            }
            else
            {
                throw new Exception("User name or Password do not match");
            }

        }

        /// <summary>
        /// This method will Register the User the by calling to User Repo Method by using Identity Methods.
        /// </summary>
        /// <param name="register"></param>
        /// <returns>Identity Result</returns>
        public Task<IdentityResult> RegisterUser(RegisterRequest register)
        {
            return userRepo.RegisterUser(register);
        }

        #endregion

        #region private methods

        /// <summary>
        /// This method Will generate the Random Password
        /// </summary>
        /// <param name="length"></param>
        /// <returns>String Password</returns>
        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        #endregion
    }
}
