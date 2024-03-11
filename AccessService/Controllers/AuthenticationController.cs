using AccessService.Models.RequestModels;
using AccessService.Models.ResponseModels;
using AccessService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccessService.Controllers
{
    /// <summary>
    /// Controller for managing Api keys and Jwt tokens
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IUserService userService, IAuthenticationService authenticationService) 
        {
            _userService = userService;
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Create Api key for current user with appropriate permissions
        /// </summary>
        /// <param name="model">Create Api key request</param>
        /// <returns>Api key</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiKeyResponse))]
        public async Task<IActionResult> CreateApiKey([FromBody] CreateApiKeyRequest model) 
        {
            var userResult = await _userService.Authenticate(model.UserId, model.Password);

            if (!userResult.HasValue) 
            {
                return Unauthorized();
            }

            var apiKey = await _authenticationService.GenerateApiKey(model);

            return Ok(apiKey);
        }

        /// <summary>
        /// Generate Jwt token using Api key
        /// </summary>
        /// <param name="apiKey">Api key (in header "X-API-Key")</param>
        /// <returns>Jwt token</returns>
        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponse))]
        public async Task<IActionResult> GenerateToken([FromHeader(Name = Constants.ApiKeyHeaderName)] string apiKey) 
        {
            var token = await _authenticationService.GenerateToken(apiKey, DateTime.UtcNow);

            return Ok(token);
        }

        /// <summary>
        /// Revoke Api key by it's Id. Only owner can revoke it's key
        /// </summary>
        /// <param name="model">Revoke Api key request</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> RevokeApiKey([FromBody] RevokeApiKeyRequest model) 
        {
            var userResult = await _userService.Authenticate(model.UserId, model.Password);

            if (!userResult.HasValue)
            {
                return Unauthorized();
            }

            await _authenticationService.RevokeApiKey(model.ApiKeyId, model.UserId);

            return NoContent();
        }

        /// <summary>
        /// Get the list of Api keys for current user
        /// </summary>
        /// <param name="model">User Id and password model</param>
        /// <returns>Api keys</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiKeyItemResponse))]
        public async Task<IActionResult> GetApiKeys([FromQuery] AuthenticatedUserRequest model) 
        {
            var userResult = await _userService.Authenticate(model.UserId, model.Password);

            if (!userResult.HasValue)
            {
                return Unauthorized();
            }

            var keys = await _authenticationService.GetApiKeys(model.UserId);

            return Ok(keys);
        }
    }
}
