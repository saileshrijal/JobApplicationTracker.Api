using JobApplicationTracker.Api.Data.Dto;
using JobApplicationTracker.Api.Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Api.Controllers;

[Route("api/jobapplication")]
public class
    JobsApplicationController(IJobApplicationService jobApplicationService) : ControllerBase
{
    [HttpGet]
    [Route("/getalljobapplication")]
    public async Task<IActionResult> GetAllJobs()
    {
        var jobApp = await jobApplicationService.GetAllJobApplicationAsync();
        return Ok(jobApp);
    }

    [HttpGet]
    [Route("/getjobapplicationbyid")]
    public async Task<IActionResult> GetJobApplicationById(int id)
    {
        var jobApp = await jobApplicationService.GetJobApplicationByIdAsync(id);
        if (jobApp == null)
        {
            return NotFound();
        }
        return Ok(jobApp);
    }

    [HttpPost]
    [Route("/submitjobapplication")]
    public async Task<IActionResult> SubmitJobApplication([FromBody] JobApplicationDto jobApplicationDto)
    {
        if (jobApplicationDto == null)
        {
            return BadRequest();
        }

        var response = await jobApplicationService.SubmitJobApplicationAsync(jobApplicationDto);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpDelete]
    [Route("/deletejobapplication")]
    public async Task<IActionResult> DeleteJobApplication(int id)
    {
        var response = await jobApplicationService.DeleteJobApplicationAsync(id);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }
}