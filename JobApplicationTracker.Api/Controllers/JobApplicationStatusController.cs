using JobApplicationTracker.Api.Data.Dto;
using JobApplicationTracker.Api.Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Api.Controllers;

[Route("api/jobapplication")]
public class
    JobsApplicationStatusController(IJobApplicationStatusService jobApplicationStatusService) : ControllerBase
{
    [HttpGet]
    [Route("/getalljobapplicationstatus")]
    public async Task<IActionResult> GetAllJobApplicationStatus()
    {
        var jobAppStat = await jobApplicationStatusService.GetAllJobApplicationStatusAsync();
        return Ok(jobAppStat);
    }

    [HttpGet]
    [Route("/getjobapplicationstatusbyid")]
    public async Task<IActionResult> GetJobApplicationStatusById(int id)
    {
        var jobAppStat = await jobApplicationStatusService.GetJobApplicationStatusByIdAsync(id);
        if (jobAppStat == null)
        {
            return NotFound();
        }
        return Ok(jobAppStat);
    }

    [HttpPost]
    [Route("/submitjobapplicationstat")]
    public async Task<IActionResult> SubmitJobApplicationStatus([FromBody] JobApplicationStatusDto jobApplicationStatusDto)
    {
        if (jobApplicationStatusDto == null)
        {
            return BadRequest();
        }

        var response = await jobApplicationStatusService.SubmitJobApplicationStatusAsync(jobApplicationStatusDto);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpDelete]
    [Route("/deletejobapplicationstatus")]
    public async Task<IActionResult> DeleteJobApplicationStatus(int id)
    {
        var response = await jobApplicationStatusService.DeleteJobApplicationStatusAsync(id);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }
}