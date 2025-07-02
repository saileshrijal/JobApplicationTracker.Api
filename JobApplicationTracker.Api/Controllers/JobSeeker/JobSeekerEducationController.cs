using JobApplicationTracker.Api.Data.Dto;
using JobApplicationTracker.Api.Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Api.Controllers.JobSeeker;

[Route("api/jobseekereducation")]
public class JobSeekerEducationController(IJobSeekersEducationService jobSeekersEducationservice) : ControllerBase
{
    [HttpGet]
    [Route("/getalljobseekereducation")]
    public async Task<IActionResult> GetAllJobSeekersEducation()
    {
        var jobEduu = await jobSeekersEducationservice.GetAllJobSeekerEducationAsync();
        return Ok(jobEduu);
    }

    [HttpGet]
    [Route("/getjobseekereducation")]
    public async Task<IActionResult> GetJobSeekerEducationsById(int id)
    {
        var jobSeekerEdu= await jobSeekersEducationservice.GetJobSeekerEducationByIdAsync(id);
        if (jobSeekerEdu == null)
        {
            return NotFound();
        }
        return Ok(jobSeekerEdu);
    }

    [HttpPost]
    [Route("/submitjobseekereducation")]
    public async Task<IActionResult> SubmitJobSeekerEducation([FromBody] JobSeekerEducationDto jobSeekerEducationDto)
    {
        if (jobSeekerEducationDto == null)
        {
            return BadRequest();
        }

        var response = await jobSeekersEducationservice.SubmitJobSeekerEducationAsync(jobSeekerEducationDto);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpDelete]
    [Route("/deletejobseekereducation")]
    public async Task<IActionResult> DeleteJobSeekerEducation(int id)
    {
        var response = await jobSeekersEducationservice.DeleteJobSeekerEducationAsync(id);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }
}