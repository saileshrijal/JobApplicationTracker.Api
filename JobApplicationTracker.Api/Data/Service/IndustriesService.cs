using Dapper;
using JobApplicationTracker.Api.Configuration;
using JobApplicationTracker.Api.Data.Dto;
using JobApplicationTracker.Api.Data.Interface;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace JobApplicationTracker.Api.Data.Service;

public class IndustriesService : IIndustriesService
{
    public async Task<IEnumerable<IndustriesDto>> GetAllIndustriesAsync()
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        var sql = """
                  SELECT IndustryId, 
                         IndustryName, 
                         Description 
                  FROM Industries
                  """;

        return await connection.QueryAsync<IndustriesDto>(sql).ConfigureAwait(false);
    }

    public async Task<IndustriesDto> GetIndustryByIdAsync(int industryId)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        // SQL query to fetch an industry by ID
        var sql = """
              SELECT IndustryId, 
                     IndustryName, 
                     Description 
              FROM Industries
              WHERE IndustryId = @IndustryId
              """;

        var parameters = new DynamicParameters();
        parameters.Add("@IndustryId", industryId, DbType.Int32);

        return await connection.QueryFirstOrDefaultAsync<IndustriesDto>(sql, parameters).ConfigureAwait(false);
    }

    public async Task<ResponseDto> SubmitIndustriesAsync(IndustriesDto industriesDto)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        string sql;

        if (industriesDto.IndustryId <= 0)
        {
            // Insert new industry (assumes IndustryId is auto-incremented)
            sql = """
        INSERT INTO Industries (IndustryName, Description)
        VALUES (@IndustryName, @Description);
        SELECT CAST(SCOPE_IDENTITY() AS INT);
        """;
        }
        else
        {
            // Update existing industry
            sql = """
        UPDATE Industries
        SET 
            IndustryName = @IndustryName,
            Description = @Description
        WHERE IndustryId = @IndustryId
        """;
        }

        var parameters = new DynamicParameters();
        parameters.Add("@IndustryId", industriesDto.IndustryId, DbType.Int32);
        parameters.Add("@IndustryName", industriesDto.IndustryName, DbType.String);
        parameters.Add("@Description", industriesDto.Description, DbType.String);

        var affectedRows = 0;

        if (industriesDto.IndustryId <= 0)
        {
            // Insert operation
            var newId = await connection.QuerySingleAsync<int>(sql, parameters).ConfigureAwait(false);
            affectedRows = newId > 0 ? 1 : 0;
            industriesDto.IndustryId = newId; // Set the ID for the newly inserted record
        }
        else
        {
            // Update operation
            affectedRows = await connection.ExecuteAsync(sql, parameters).ConfigureAwait(false);
        }

        return new ResponseDto
        {
            IsSuccess = affectedRows > 0,
            Message = affectedRows > 0 ? "Industries submitted successfully." : "Failed to submit industries.",
           
        };
    }

    public async Task<ResponseDto> DeleteIndustriesAsync(int industryId)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();
        // write the SQL query to delete a job type by ID
        var sql = """DELETE FROM Industries WHERE IndustryId = IndustryIdId""";

        var parameters = new DynamicParameters();
        parameters.Add("@IndustryIdId", industryId, DbType.Int32);

        var affectedRows = await connection.ExecuteAsync(sql, parameters).ConfigureAwait(false);

        if (affectedRows <= 0)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Failed to delete industry."
            };
        }

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "Industry deleted successfully."
        };
    }
}
