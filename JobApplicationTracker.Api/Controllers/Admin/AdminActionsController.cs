using JobApplicationTracke.Data.Dto;
using JobApplicationTracke.Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Api.Controllers.Admin;

[Route("api/AdminActions")]
public class AdminActionsController(IAdminActionRepository adminActionService) : ControllerBase
{
    [HttpGet]
    [Route("/getalladminActions")]
    public async Task<IActionResult> GetAllAdminActions()
    {
        var adminAction = await adminActionService.GetAllAdminActionAsync();
        return Ok(adminAction);
    }

    [HttpGet]
    [Route("/getadminActionbyid")]
    public async Task<IActionResult> GetAdminActionById(int id)
    {
        var adminActionn = await adminActionService.GetAdminActionByIdAsync(id);
        if (adminActionn == null)
        {
            return NotFound();
        }
        return Ok(adminActionn);
    }

    [HttpPost]
    [Route("/submitAdminAction")]
    public async Task<IActionResult> SubmitAdminAction([FromBody] AdminActionsDto adminActionDto)
    {
        if (adminActionDto == null)
        {
            return BadRequest();
        }

        var response = await adminActionService.SubmitAdminActionAsync(adminActionDto);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpDelete]
    [Route("/deleteAdminAction")]
    public async Task<IActionResult> DeleteAdminAction(int id)
    {
        var response = await adminActionService.DeleteAdminActionAsync(id);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }
}