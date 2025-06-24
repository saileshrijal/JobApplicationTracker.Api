using JobApplicationTracker.Api.Data.Dto;
using JobApplicationTracker.Api.Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Api.Controllers;

[Route("api/usertypes")]
public class UserTypesController(IUsersTypeService userTypesService) : ControllerBase
{
    [HttpGet]
    [Route("/getallusertypes")]
    public async Task<IActionResult> GetAllUserTypes()
    {
        var userTypes = await userTypesService.GetAllUserTypesAsync();
        return Ok(userTypes);
    }

    [HttpGet]
    [Route("/getusertypesbyid")]
    public async Task<IActionResult> GetUserTypesById(int id)
    {
        var userTypes = await userTypesService.GetUserTypesByIdAsync(id);
        if (userTypes == null)
        {
            return NotFound();
        }
        return Ok(userTypes);
    }

    [HttpPost]
    [Route("/submitusertypes")]
    public async Task<IActionResult> SubmitUserTypes([FromBody] UserTypesDto userTypesDto)
    {
        if (userTypesDto == null)
        {
            return BadRequest();
        }

        var response = await userTypesService.SubmitUserTypesAsync(userTypesDto);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpDelete]
    [Route("/deleteusertypes")]
    public async Task<IActionResult> DeleteUserTypes(int id)
    {
        var response = await userTypesService.DeleteUserTypesAsync(id);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }
}