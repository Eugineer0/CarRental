using Microsoft.AspNetCore.Mvc;
using CarRentalApp.Models.DTOs.Requests;
using CarRentalApp.Services.Authentication;

namespace CarRentalApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Incorrect format");
            } 

            var authResponse = await _authenticationService.Authenticate(model);
            if (authResponse == null)
            {
                return Unauthorized("Incorrect username or password");
            }

            return Ok(authResponse);
        }        
    }
}