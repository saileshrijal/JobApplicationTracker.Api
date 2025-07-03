using JobApplicationTracke.Data.Dto;
using JobApplicationTracke.Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Api.Controllers.JobSeeker;

[Route("api/jobSeekers")]
public class 
    JobsSeekersController(IJobSeekersRepository jobSeekerService) : ControllerBase
{
    [HttpGet]
    [Route("/getalljobSeekers")]
    public async Task<IActionResult> GetAllJobSeekers()
    {
        var jobSeeker = await jobSeekerService.GetAllJobSeekersAsync();
        return Ok(jobSeeker);
    }

    [HttpGet]
    [Route("/getjobSeekerbyid")]
    public async Task<IActionResult> GetJobSeekerById(int id)
    {
        var jobSeekerr = await jobSeekerService.GetJobSeekersByIdAsync(id);
        if (jobSeekerr == null)
        {
            return NotFound();
        }
        return Ok(jobSeekerr);
    }

    [HttpPost]
    [Route("/submitjobSeeker")]
    public async Task<IActionResult> SubmitJobSeeker([FromBody] JobSeekersDto jobSeekersDto)
    {
        if (jobSeekersDto == null)
        {
            return BadRequest();
        }

        var response = await jobSeekerService.SubmitJobSeekersAsync(jobSeekersDto);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpDelete]
    [Route("/deletejobseeker")]
    public async Task<IActionResult> DeleteJobSeeker(int id)
    {
        var response = await jobSeekerService.DeleteJobSeekersAsync(id);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }
}