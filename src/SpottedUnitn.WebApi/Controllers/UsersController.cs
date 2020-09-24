using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpottedUnitn.Data.Dto.User;
using SpottedUnitn.Model.Exceptions;
using SpottedUnitn.Services.Dto.User;
using SpottedUnitn.Services.Interfaces;
using SpottedUnitn.WebApi.Authorization;
using SpottedUnitn.WebApi.ErrorHandling;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpottedUnitn.WebApi.Controllers
{
    [Route("users")]
    [Authorize]
    [ApiController]
    public class UsersController : EntityController
    {
        protected IUserService userService;
        protected ILogger<UsersController> logger;

        public UsersController(ILogger<UsersController> logger,  IUserService userService, ICustomExceptionHandler excHandler) : base(excHandler)
        {
            this.userService = userService;
            this.logger = logger;
        }

        // GET: users
        [HttpGet]
        [Authorize(Policy = AuthorizationOptionsExtension.onlyAdminPolicy)]
        [Produces("application/json")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<UserBasicInfoDto>>> GetUsersAsync()
        {
            return await this.userService.GetUsersAsync();
        }

        // GET users/me
        [HttpGet("me")]
        [Authorize(Policy = AuthorizationOptionsExtension.onlyRegisteredOrAdminPolicy)]
        [Produces("application/json")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserBasicInfoDto>> GetCurrentUserAsync()
        {
            try
            {
                return await this.userService.GetUserInfoAsync(int.Parse(User.Identity.Name));
            }
            catch (UserException exc) when (exc.Code == (int)UserException.UserExceptionCode.UserIdNotFound)
            {
                return NotFound(exc);
            }
        }

        // POST users/login
        [HttpPost("login")]
        [AllowAnonymous]
        [Produces("application/json")]
        [Consumes("application/x-www-form-urlencoded")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status409Conflict)]
        public async Task<ActionResult<AuthenticatedUserDto>> LoginUserAsync([FromForm] UserCredentialsDto userCredentials)
        {
            try
            {
                return await this.userService.LoginAsync(userCredentials);
            }
            catch (UserException exc) when (exc.HasCodeIn((int)UserException.UserExceptionCode.WrongMail, (int)UserException.UserExceptionCode.WrongPassword))
            {
                return BadRequest(exc);
            }
            catch (UserException exc) when (exc.Code == (int)UserException.UserExceptionCode.UserNotConfirmed)
            {
                return Conflict(exc);
            }
        }

        // POST users
        [HttpPost]
        [AllowAnonymous]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RegisterUserAsync([FromForm] UserRegisterDto userRegister)
        {
            try
            {
                await this.userService.AddUserAsync(userRegister);
                return Ok();
            }
            catch (UserException exc) when (exc.HasCodeIn(
                (int)UserException.UserExceptionCode.DuplicateMail,
                (int)UserException.UserExceptionCode.InvalidName,
                (int)UserException.UserExceptionCode.InvalidLastName,
                (int)UserException.UserExceptionCode.InvalidMail,
                (int)UserException.UserExceptionCode.InvalidPassword,
                (int)UserException.UserExceptionCode.InvalidProfilePhoto))
            {
                return BadRequest(exc);
            }
        }

        // PUT users/5/confirm
        [HttpPut("{userId}/confirm")]
        [Authorize(Policy = AuthorizationOptionsExtension.onlyAdminPolicy)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(statusCode: StatusCodes.Status409Conflict)]
        public async Task<ActionResult> ConfirmUserAsync(int userId)
        {
            try
            {
                await this.userService.ConfirmUserRegistrationAsync(userId);
                return Ok();
            }
            catch (UserException exc) when (exc.Code == (int)UserException.UserExceptionCode.UserIdNotFound)
            {
                return NotFound(exc);
            }
            catch (UserException exc) when (exc.Code == (int)UserException.UserExceptionCode.CannotConfirmRegistration)
            {
                return Conflict(exc);
            }
        }

        // DELETE users/me
        [HttpDelete("me")]
        [Authorize(Policy = AuthorizationOptionsExtension.onlyRegisteredOrAdminPolicy)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteUserAsync()
        {
            try
            {
                await this.userService.DeleteUserAsync(int.Parse(User.Identity.Name));
                return Ok();
            }
            catch (UserException exc) when (exc.Code == (int)UserException.UserExceptionCode.UserIdNotFound)
            {
                return NotFound(exc);
            }
        }

        // GET users/me
        [HttpGet("me/profilePhoto")]
        [Produces("application/octet-stream")]
        [Authorize(Policy = AuthorizationOptionsExtension.onlyRegisteredOrAdminPolicy)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<ActionResult<byte[]>> GetUserProfilePhotoAsync()
        {
            try
            {
                return await this.userService.GetUserProfilePhotoAsync(int.Parse(User.Identity.Name));
            }
            catch (UserException exc) when (exc.Code == (int)UserException.UserExceptionCode.UserIdNotFound)
            {
                return NotFound(exc);
            }
        }
    }
}
