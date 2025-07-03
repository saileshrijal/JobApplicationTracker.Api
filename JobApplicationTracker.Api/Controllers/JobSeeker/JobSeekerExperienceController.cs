using JobApplicationTracke.Data.Dto;
using JobApplicationTracke.Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Api.Controllers.JobSeeker;

[Route("api/jobseekerexperience")]
public class JobSeekerExperienceController(IJobSeekerExperienceRepository jobSeekersExperienceService) : ControllerBase
{
    [HttpGet]
    [Route("/getalljobseekerexperience")]
    public async Task<IActionResult> GetAllJobSeekersExperience()
    {
        var jobExp = await jobSeekersExperienceService.GetAllJobSeekerExperienceAsync();
        return Ok(jobExp);
    }

    [HttpGet]
    [Route("/getjobseekerexperience")]
    public async Task<IActionResult> GetJobSeekerExperienceById(int id)
    {
        var jobSeekerExp = await jobSeekersExperienceService.GetJobSeekerExperienceByIdAsync(id);
        if (jobSeekerExp == null)
        {
            return NotFound();
        }
        return Ok(jobSeekerExp);
    }

    [HttpPost]
    [Route("/submitjobseekerexperience")]
    public async Task<IActionResult> SubmitJobSeekerExperience([FromBody] JobSeekerExperienceDto jobSeekerExperienceDto)
    {
        if (jobSeekerExperienceDto == null)
        {
            return BadRequest();
        }

        var response = await jobSeekersExperienceService.SubmitJobSeekerExperienceAsync(jobSeekerExperienceDto);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpDelete]
    [Route("/deletejobseekerexperience")]
    public async Task<IActionResult> DeleteJobSeekerExperience(int id)
    {
        var response = await jobSeekersExperienceService.DeleteJobSeekerExperienceAsync(id);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }
}