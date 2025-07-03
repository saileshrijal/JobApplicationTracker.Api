using JobApplicationTracke.Data.Dto;
using JobApplicationTracke.Data.Interface;
using JobApplicationTracker.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Api.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IUserRepository _usersService, ICookieService _cookieService, 
        IAuthenticationService _authenticationService) : ControllerBase
    {
        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login(LoginDto credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (credentials == null)
            {
                return BadRequest(ModelState);
            }

            var user = await _usersService.GetUserByEmail(credentials.Email);

            // check for user existance
            if (user == null)
            {
                return BadRequest(new ResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "User does not exist. Please sign up to login."
                });
            }

            // match the credentials
            if (!BCrypt.Net.BCrypt.Verify(credentials.Password, user.PasswordHash))
            {
                return BadRequest(new ResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = "Incorrect Password. Please try again later."
                });
            }

            var jwtToken = _authenticationService.GenerateJwtToken(user);
            _cookieService.AppendCookies(Response, jwtToken);

            return Ok(new ResponseDto()
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "Login Successful."
            });
        }
    }
}
