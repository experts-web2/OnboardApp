using AppServices.IServices;
using DomainEntities;
using DTOs.RequestDtos;
using DTOs.ResponseDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
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
        public AuthenticationController(IUserService userService)
        {
            this.userService = userService;
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
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = await userService.GetUserByEmailAsync(request.Email);
                    if (user != null)
                    {
                        string token = await userService.GeneratePasswordResetTokenAsync(user);
                        // we will change url for hosted url then
                        var url = $"https://localhost:55179/resetpassword?email={request.Email}&token={token}";
                        SendMailFunc(token, "testuserg38@gmail.com", url, "");
                    }
                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }
            }
            else
            {
                // If we got this far, something failed, redisplay form
                return BadRequest(ModelState);
            }
            return Ok();
        }

        private static string SendMailFunc(string subject, string recepient, string message, string filePath)
        {
            try
            {
                
                 
                string From_Mail = "testuserg38@gmail.com";
                string From_Password = "Test123321";
                MailAddress fromAddress = new MailAddress(From_Mail);
                MailAddress toAddress = new MailAddress(recepient);
                MailMessage mail = new MailMessage(fromAddress.Address, toAddress.Address)
                {
                    Subject = subject,
                    Body = message
                };

                
                SmtpClient client = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    Timeout = 50000,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(From_Mail, From_Password)
                };

                client.Send(mail);
                return "Mail sent";
            }
            catch (Exception)
            {
            }
            return "Error occured";
        }

    }
}
