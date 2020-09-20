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
using SpottedUnitn.Services.Dto.User;
using SpottedUnitn.Services.Interfaces;
using SpottedUnitn.WebApi.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpottedUnitn.WebApi.Controllers
{
    [Route("users")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        protected IUserService userService;
        protected ILogger<UsersController> logger;

        public UsersController(ILogger<UsersController> logger,  IUserService userService)
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
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserBasicInfoDto>> GetCurrentUserAsync()
        {
            return await this.userService.GetUserInfoAsync(int.Parse(User.Identity.Name));
        }

        // POST users/login
        [HttpPost("login")]
        [AllowAnonymous]
        [Produces("application/json")]
        [Consumes("application/x-www-form-urlencoded")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<AuthenticatedUserDto>> LoginUserAsync([FromForm] UserCredentialsDto userCredentials)
        {
            return await this.userService.LoginAsync(userCredentials);
        }

        // POST users
        [HttpPost]
        [AllowAnonymous]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        public async Task RegisterUserAsync([FromForm] UserRegisterDto userRegister)
        {
            await this.userService.AddUserAsync(userRegister);
        }

        // PUT users/5/confirm
        [HttpPut("{userId}/confirm")]
        [Authorize(Policy = AuthorizationOptionsExtension.onlyAdminPolicy)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(statusCode: StatusCodes.Status409Conflict)]
        public async Task ConfirmUserAsync(int userId)
        {
            await this.userService.ConfirmUserRegistrationAsync(userId);
        }

        // DELETE users/me
        [HttpDelete("me")]
        [Authorize(Policy = AuthorizationOptionsExtension.onlyRegisteredOrAdminPolicy)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task DeleteUserAsync()
        {
            await this.userService.DeleteUserAsync(int.Parse(User.Identity.Name));
        }

        // GET users/me
        [HttpGet("me/profilePhoto")]
        [Produces("application/octet-stream")]
        [Authorize(Policy = AuthorizationOptionsExtension.onlyRegisteredOrAdminPolicy)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<ActionResult<byte[]>> GetUserProfilePhotoAsync()
        {
            return await this.userService.GetUserProfilePhotoAsync(int.Parse(User.Identity.Name));
        }
    }
}
