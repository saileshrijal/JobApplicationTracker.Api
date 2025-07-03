using JobApplicationTracke.Data.Dto;
using JobApplicationTracke.Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Api.Controllers.Admin;

[Route("api/AdminLogs")]
public class AdminLogsController(IAdminLogsRepository adminLogRepository) : ControllerBase
{
    [HttpGet]
    [Route("/getallAdminLogs")]
    public async Task<IActionResult> GetAllAdminLogs()
    {
        var adminLog = await adminLogRepository.GetAllAdminLogsAsync();
        return Ok(adminLog);
    }

    [HttpGet]
    [Route("/getadminLogsbyid")]
    public async Task<IActionResult> GetAdminLogsById(int id)
    {
        var adminLog = await adminLogRepository.GetAdminLogsByIdAsync(id);
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

        var response = await adminLogRepository.SubmitAdminLogsAsync(adminLogsDto);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpDelete]
    [Route("/deleteAdminLog")]
    public async Task<IActionResult> DeleteAdminLog(int id)
    {
        var response = await adminLogRepository.DeleteAdminLogsAsync(id);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }
}