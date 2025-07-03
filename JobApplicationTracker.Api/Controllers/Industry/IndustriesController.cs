using JobApplicationTracke.Data.Dto;
using JobApplicationTracke.Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Api.Controllers.Industry;

[Route("api/industries")]
public class
    IndustriesController(IIndustriesRepository industriesService) : ControllerBase
{
    [HttpGet]
    [Route("/getallindustries")]
    public async Task<IActionResult> GetAllIndustries()
    {
        var industries = await industriesService.GetAllIndustriesAsync();
        return Ok(industries);
    }

    [HttpGet]
    [Route("/getindustriesbyid")]
    public async Task<IActionResult> GetIndustriesById(int id)
    {
        var ind = await industriesService.GetIndustryByIdAsync(id);
        if (ind == null)
        {
            return NotFound();
        }
        return Ok(ind);
    }

    [HttpPost]
    [Route("/submitindustries")]
    public async Task<IActionResult> SubmitIndustries([FromBody] IndustriesDto industriesDto)
    {
        if (industriesDto == null)
        {
            return BadRequest();
        }

        var response = await industriesService.SubmitIndustriesAsync(industriesDto);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpDelete]
    [Route("/deleteindustries")]
    public async Task<IActionResult> DeleteIndustries(int id)
    {
        var response = await industriesService.DeleteIndustriesAsync(id);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }
}