using Dapper;
using JobApplicationTracker.Api.Configuration;
using JobApplicationTracker.Api.Data.Dto;
using JobApplicationTracker.Api.Data.Interface;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace JobApplicationTracker.Api.Data.Service;

public class CompaniesService : ICompaniesService
{
    public async Task<IEnumerable<CompaniesDto>> GetAllCompaniesAsync()
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        var sql = """
                  SELECT CompanyId,
                         CompanyName,
                         CompanyLogo,
                         IndustryId,
                         CompanySizeId,
                         Website,
                         Location,
                         Description,
                         CreatedAt,
                         UpdatedAt
                  FROM Companies
                  """;

        return await connection.QueryAsync<CompaniesDto>(sql).ConfigureAwait(false);
    }

    public async Task<CompaniesDto> GetCompaniesByIdAsync(int companiesId)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        var sql = """
              SELECT CompanyId,
                     UserId,
                     CompanyName,
                     CompanyLogo,
                     IndustryId,
                     CompanySizeId,
                     Website,
                     Location,
                     Description,
                     CreatedAt,
                     UpdatedAt
              FROM Companies
              WHERE CompanyId = @companiesId
              """;

        var parameters = new DynamicParameters();
        parameters.Add("@companiesId", companiesId, DbType.Int32);

        return await connection.QueryFirstOrDefaultAsync<CompaniesDto>(sql, parameters).ConfigureAwait(false);
    }
    public async Task<ResponseDto> SubmitCompaniesAsync(CompaniesDto companiesDto)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        string sql;
        bool isInsert = companiesDto.CompanyId <= 0;

        if (isInsert)
        {
            // Insert new company
            sql = """
        INSERT INTO Companies (
            UserId,
            CompanyName, 
            CompanyLogo,
            IndustryId,
            CompanySizeId,
            Website,
            Location,
            Description
        )
        VALUES (
            @UserId,
            @CompanyName,
            @CompanyLogo,
            @IndustryId,
            @CompanySizeId,
            @Website,
            @Location,
            @Description
        );
        SELECT CAST(SCOPE_IDENTITY() AS INT);
        """;
        }
        else
        {
            // Update existing company
            sql = """
        UPDATE Companies
        SET 
            CompanyName = @CompanyName,
            CompanyLogo = @CompanyLogo,
            IndustryId = @IndustryId,
            CompanySizeId = @CompanySizeId,
            Website = @Website,
            Location = @Location,
            Description = @Description,
            UpdatedAt = GETUTCDATE()
        WHERE CompanyId = @CompanyId
        """;
        }

        var parameters = new DynamicParameters();
        parameters.Add("@CompanyId", companiesDto.CompanyId, DbType.Int32);
        parameters.Add("@UserId", companiesDto.CompanyId, DbType.Int32);
        parameters.Add("@CompanyName", companiesDto.CompanyName, DbType.String);
        parameters.Add("@CompanyLogo", companiesDto.CompanyLogo, DbType.String);
        parameters.Add("@IndustryId", companiesDto.IndustryId, DbType.Int32);
        parameters.Add("@CompanySizeId", companiesDto.CompanySizeId, DbType.Int32);
        parameters.Add("@Website", companiesDto.Website, DbType.String);
        parameters.Add("@Location", companiesDto.Location, DbType.String);
        parameters.Add("@Description", companiesDto.Description, DbType.String);

        var affectedRows = 0;

        if (isInsert)
        {
            var newId = await connection.QuerySingleAsync<int>(sql, parameters).ConfigureAwait(false);
            affectedRows = newId > 0 ? 1 : 0;
            companiesDto.CompanyId = newId;
            companiesDto.CreatedAt = DateTime.UtcNow;
        }
        else
        {
            affectedRows = await connection.ExecuteAsync(sql, parameters).ConfigureAwait(false);
            companiesDto.UpdatedAt = DateTime.UtcNow;
        }

        return new ResponseDto
        {
            IsSuccess = affectedRows > 0,
            Message = affectedRows > 0
                ? (isInsert ? "Company created successfully." : "Company updated successfully.")
                : "Failed to submit company.",
            
        };
    }
    public async Task<ResponseDto> DeleteCompanyAsync(int companiesId)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        // Check referential integrity first (example for JobApplications table)
        var refCheckSql = "SELECT COUNT(1) FROM JobApplications WHERE CompanyId = @CompanyId";
        var hasDependencies = await connection.ExecuteScalarAsync<bool>(refCheckSql, new { CompanyId = companiesId });

        if (hasDependencies)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Cannot delete company because it has associated job applications."
            };
        }

        // Delete the company
        var deleteSql = "DELETE FROM Companies WHERE CompanyId = @CompanyId";

        var parameters = new DynamicParameters();
        parameters.Add("@CompanyId", companiesId, DbType.Int32);

        var affectedRows = await connection.ExecuteAsync(deleteSql, parameters).ConfigureAwait(false);

        if (affectedRows <= 0)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Company not found or could not be deleted."
            };
        }

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "Company deleted successfully.",

        };
    }

}