using AppServices.EmailService;
using AppServices.IServices;
using DomainEntities;
using DTOs.RequestDtos;
using DTOs.ResponseDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OnboardingApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        IUserService userService;
        private Random random = new Random();
        private readonly IEmailSender _emailSender;
        public AuthenticationController(IUserService userService, IEmailSender _emailSender)
        {
            this.userService = userService;
            this._emailSender = _emailSender;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterRequest register)
        {
            try
            {
                var result = await userService.RegisterUser(register);
                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    throw new Exception("Bad Request");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            try
            {
                var result = await userService.LoginUser(loginRequest);
                if (result.Succeeded)
                {
                    User user = await userService.GetUserByEmailAsync(loginRequest.Email);
                    return Ok( new LoginResponse
                    {
                        UserId = user.Id,
                        Token = userService.GenerateJSONWebToken(user.UserName),
                        Email = user.Email,
                        CreatedDate = user.CreatedDate
                    });
                }
                else
                {
                    return BadRequest("Credential Issue");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = await userService.GetUserByEmailAsync(request.Email);
                    if (user != null)
                    {
                        string token = await userService.GeneratePasswordResetTokenAsync(user);
                        string password = this.RandomString(8);
                        var resetPassResult = await userService.ResetPasswordAsync(user, token, password);

                        var message = new Message(new string[] { user.Email }, "New password", "Your New Password is: " + password, null);
                        await _emailSender.SendEmailAsync(message);
                    }
                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }
            }
            else
            {
                // If we got this far, something failed
                return BadRequest(ModelState);
            }
            return Ok();
        }

   
        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
