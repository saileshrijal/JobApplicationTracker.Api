using JobApplicationTracke.Data.Dto;
using JobApplicationTracke.Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Api.Controllers.Company;

[Route("api/companyies")]
public class CompaniesController(ICompaniesRepository companyService) : ControllerBase
{
    [HttpGet]
    [Route("/getallcompanies")]
    public async Task<IActionResult> GetAllCompanies()
    {
        var company = await companyService.GetAllCompaniesAsync();
        return Ok(company);
    }

    [HttpGet]
    [Route("/getcompanybyid")]
    public async Task<IActionResult> GetCompanyById(int id)
    {
        var company = await companyService.GetCompaniesByIdAsync(id);
        if (company == null)
        {
            return NotFound();
        }
        return Ok(company);
    }

    [HttpPost]
    [Route("/submitcompany")]
    public async Task<IActionResult> SubmitCompany([FromBody] CompaniesDto companyDto)
    {
        if (companyDto == null)
        {
            return BadRequest();
        }

        var response = await companyService.SubmitCompaniesAsync(companyDto);
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
        var response = await companyService.DeleteCompanyAsync(id);
        if (response is ResponseDto responseDto) // Ensure the response is cast to the correct type
        {
            return responseDto.IsSuccess ? Ok(responseDto) : BadRequest(responseDto);
        }
        return BadRequest("Invalid response type.");
    }
}