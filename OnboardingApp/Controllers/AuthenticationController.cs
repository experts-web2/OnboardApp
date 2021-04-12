using OnboardingApp.Infrastructure.Interfaces;
using DTOs.RequestDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using OnboardingApp.Infrastructure;
using Microsoft.AspNetCore.Identity;
using AppRepo.UnitOfWork;

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

        #region public methods

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
                IdentityResult result = await userService.RegisterUser(register);
                if(result.Succeeded)
                    return Ok(SetResponse(new {}, true));
                else
                    return BadRequest(SetResponse(new {}, false, string.Join(", ", result.Errors.Select(x=>x.Description))));
            }
            catch (Exception ex)
            {
                return BadRequest(SetResponse(new {}, false, ex.Message));
            }
        }

        /// <summary>
        /// This Endpoint will Login the User after taking valid request
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            try
            {
                var result = await userService.LoginUser(loginRequest);
                return Ok(SetResponse(result, true));
            }

            catch (Exception ex)
            {
                return BadRequest(SetResponse(new {}, false, ex.Message));
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
        [Route("resetpassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {

            try
            {
                 await userService.ResetPasswordAsync(request.Email);
                 return Ok(SetResponse(new {message= "Password has been sent on your email address" }, true));

            }
            catch (Exception ex)
            {
                return BadRequest(SetResponse(new { }, false, ex.Message));
            }
        }

        #endregion

        #region private methods

        private static Response<T> SetResponse<T>(T responseData, bool isSuccess, string errorMessage = null)
        {
            return new Response<T>
            {
                Data = responseData,
                IsSuccess = isSuccess,
                ErrorMessage = errorMessage
            };
        }

        #endregion
    }
}