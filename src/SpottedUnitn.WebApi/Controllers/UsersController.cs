using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpottedUnitn.Data.Dto.User;
using SpottedUnitn.Services.Dto.User;
using SpottedUnitn.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpottedUnitn.WebApi.Controllers
{
    [Route("users")]
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
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<UserBasicInfoDto>>> GetUsersAsync()
        {
            return null;
        }

        // GET users/me
        [HttpGet("me")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserBasicInfoDto>> GetCurrentUserAsync()
        {
            return null;
        }

        // POST users/login
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<LoggedInUserDto>> LoginUserAsync([FromBody] UserCredentialsDto userCredentials)
        {
            return null;
        }

        // POST users
        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        public async Task RegisterUserAsync([FromBody] UserRegisterDto userRegister)
        {
        }

        // PUT users/5/confirm
        [HttpPut("{userId}/confirm")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(statusCode: StatusCodes.Status409Conflict)]
        public async Task ConfirmUserAsync(int userId)
        {
        }

        // DELETE users/me
        [HttpDelete("me")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task DeleteUserAsync()
        {
        }

        // GET users/me
        [HttpGet("me/profilePhoto")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<ActionResult<byte[]>> GetUserProfilePhotoAsync()
        {
            return null;
        }
    }
}
