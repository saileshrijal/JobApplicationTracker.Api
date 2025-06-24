using JobApplicationTracker.Api.Data.Dto;
using JobApplicationTracker.Api.Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Api.Controllers;

[Route("api/applications")]
public class ApplicationsController(IJobApplicationService jobApplicationService) : ControllerBase
{
    [HttpGet]
    [Route("/getallapplications")]
    public async Task<IActionResult> GetAllApplications()
    {
        var company = await jobApplicationService.GetAllJobApplicationsAsync();
        return Ok(company);
    }

    [HttpGet]
    [Route("/getapplicationbyid")]
    public async Task<IActionResult> GetApplicationById(int id)
    {
        var jobApplication = await ApplicationsService.GetJobApplicationByIdAsync(id);
        if (jobApplication == null)
        {
            return NotFound();
        }
        return Ok(jobApplication);
    }

    [HttpPost]
    [Route("/submitJobApplication")]
    public async Task<IActionResult> SubmitJobApplication([FromBody] JobApplicationDto jobApplicationDto)
    {
        if (jobApplicationDto == null)
        {
            return BadRequest();
        }

        var response = await jobApplicationService.SubmitJobApplicationAsync(jobApplicationDto);
        if (response is ResponseDto responseDto) // Ensure the response is cast to the correct type
        {
            return responseDto.IsSuccess ? Ok(responseDto) : BadRequest(responseDto);
        }
        return BadRequest("Invalid response type.");
    }

    [HttpDelete]
    [Route("/deletecompany")]
    public async Task<IActionResult> DeleteCompany(int id)
    {
        var response = await companyService.DeleteCompaniesAsync(id);
        if (response is ResponseDto responseDto) // Ensure the response is cast to the correct type
        {
            return responseDto.IsSuccess ? Ok(responseDto) : BadRequest(responseDto);
        }
        return BadRequest("Invalid response type.");
    }
}