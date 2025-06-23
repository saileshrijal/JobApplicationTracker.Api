using JobApplicationTracker.Api.Data.Dto;
using JobApplicationTracker.Api.Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Api.Controllers;

[Route("api/AdminLogs")]
public class AdminLogsController(IAdminLogService adminLogService) : ControllerBase
{
    [HttpGet]
    [Route("/getallAdminLogs")]
    public async Task<IActionResult> GetAllAdminLogs()
    {
        var adminLog = await adminLogService.GetAllAdminLogsAsync();
        return Ok(adminLog);
    }

    [HttpGet]
    [Route("/getadminLogsbyid")]
    public async Task<IActionResult> GetAdminLogsById(int id)
    {
        var adminLog = await adminLogService.GetAdminLogsByIdAsync(id);
        if (adminLog == null)
        {
            return NotFound();
        }
        return Ok(adminLog);
    }

    [HttpPost]
    [Route("/submitAdminLogs")]
    public async Task<IActionResult> SubmitAdminLogs([FromBody] AdminLogsDto adminLogsDto)
    {
        if (adminLogsDto == null)
        {
            return BadRequest();
        }

        var response = await adminLogService.SubmitAdminLogsAsync(adminLogsDto);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpDelete]
    [Route("/deleteAdminLog")]
    public async Task<IActionResult> DeleteAdminLog(int id)
    {
        var response = await adminLogService.DeleteAdminLogsAsync(id);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }
}