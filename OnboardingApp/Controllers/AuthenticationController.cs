using AppServices.EmailService;
using AppServices.Interfaces;
using DomainEntities;
using DTOs.RequestDtos;
using DTOs.ResponseDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService userService;

        public AuthenticationController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// This Endpoint will take the Request to Register the User and it will be registered the User.
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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
                    return Conflict(result.Errors.Select(x => x.Description));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// This Endpoint will Login the User after taking valid request
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            try
            {
                var result = await userService.LoginUser(loginRequest);
                return Ok(result);
               
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets email in request
        /// and sends the Password to his Email Address. 
        /// </summary>
        /// <param name="request">ResetPasswordRequest</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("ResetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {

            try
            {
                await userService.ResetPasswordAsync(request.Email);
                return Ok("Password has been sent on your email address");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }


}

