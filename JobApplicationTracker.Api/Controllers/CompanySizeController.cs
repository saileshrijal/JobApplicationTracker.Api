using JobApplicationTracker.Api.Data.Dto;
using JobApplicationTracker.Api.Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Api.Controllers;

[Route("api/companyiessize")]
public class CompaniesSizeController(ICompanySizesService companyService) : ControllerBase
{
    [HttpGet]
    [Route("/getallcompaniessize")]
    public async Task<IActionResult> GetGetAllCompanySize()
    {
        var companysize = await companyService.GetAllCompanySizeAsync();
        return Ok(companysize);
    }

    [HttpGet]
    [Route("/getcompanysizebyid")]
    public async Task<IActionResult> GetCompanySizeById(int id)
    {
        var companySize = await companyService.GetCompanySizeByIdAsync(id);
        if (companySize == null)
        {
            return NotFound();
        }
        return Ok(companySize);
    }

    [HttpPost]
    [Route("/submitcompanysize")]
    public async Task<IActionResult> SubmitCompanySize([FromBody] CompanySizeDto companySizeDto)
    {
        if (companySizeDto == null)
        {
            return BadRequest();
        }

        var response = await companyService.SubmitCompanySizeAsync(companySizeDto);
        if (response is ResponseDto responseDto) // Ensure the response is cast to the correct type
        {
            return responseDto.IsSuccess ? Ok(responseDto) : BadRequest(responseDto);
        }
        return BadRequest("Invalid response type.");
    }

    [HttpDelete]
    [Route("/deletecompanysize")]
    public async Task<IActionResult> DeleteCompanySize(int id)
    {
        var response = await companyService.DeleteCompanySizeAsync(id);
        if (response is ResponseDto responseDto) // Ensure the response is cast to the correct type
        {
            return responseDto.IsSuccess ? Ok(responseDto) : BadRequest(responseDto);
        }
        return BadRequest("Invalid response type.");
    }
}