using JobApplicationTracker.Api.Data.Dto;
using JobApplicationTracker.Api.Data.Interface;
using JobApplicationTracker.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Api.Controllers.User;

[Route("api/users")]
public class UsersController(IUsersService userService, IPasswordHasherService _passwordHasher) : ControllerBase
{
    [HttpGet]
    [Route("/getallusers")]
    public async Task<IActionResult> GetAllJobTypes()
    {
        var users = await userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet]
    [Route("/getusersbyid")]
    public async Task<IActionResult> GetUsersById(int id)
    {
        var userr = await userService.GetUsersByIdAsync(id);
        if (userr == null)
        {
            return NotFound();
        }
        return Ok(userr);
    }

    [HttpPost]
    [Route("/submitusers")]
    public async Task<IActionResult> SubmitUsers([FromBody] UsersDto usersDto)
    {
        if (usersDto == null)
        {
            return BadRequest();
        }

        usersDto.PasswordHash = _passwordHasher.HashPassword(usersDto.PasswordHash);

        var response = await userService.SubmitUsersAsync(usersDto);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpDelete]
    [Route("/deleteuser")]
    public async Task<IActionResult> DeleteJobType(int id)
    {
        var response = await userService.DeleteUsersAsync(id);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpPost]
    [Route("/signup")]
    public async Task<IActionResult> CreateUser(SignupDto credentials)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("Error","Error occured.Please enter all credentails.");
            return BadRequest(ModelState);
        }

        if (credentials is null)
        {
            return BadRequest();
        }

        // check if the user already exists
        if (await userService.DoesEmailExists(credentials.Email))
        {
            return BadRequest(new ResponseDto()
            {   
                IsSuccess = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "The email already exists."
            });
        }

        // hash the password
        credentials.PasswordHash = _passwordHasher.HashPassword(credentials.PasswordHash);

        // return ok response
        var response = await userService.CreateUserAsync(credentials);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }
    
}