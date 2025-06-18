using Dapper;
using JobApplicationTracker.Api.Configuration;
using JobApplicationTracker.Api.Data.Dto;
using JobApplicationTracker.Api.Data.Interface;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace JobApplicationTracker.Api.Data.Service;

public class JobTypeService : IJobTypeService
{
    public async Task<IEnumerable<JobTypeDto>> GetAllJobTypesAsync()
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        var sql = """
                  SELECT JobTypeId, 
                         TypeName, 
                         Description 
                  FROM JobTypes
                  """;

        return await connection.QueryAsync<JobTypeDto>(sql).ConfigureAwait(false);
    }

    public async Task<JobTypeDto> GetJobTypeByIdAsync(int jobTypeId)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();
        // write the SQL query to fetch a job type by ID
        var sql = """
                  SELECT JobTypeId, 
                         TypeName, 
                         Description 
                  FROM JobTypes
                  WHERE JobTypeId = ?
                  """; 

        var parameters = new DynamicParameters();
        parameters.Add("@JobTypeId", jobTypeId, DbType.Int32);

        return await connection.QueryFirstOrDefaultAsync<JobTypeDto>(sql, parameters).ConfigureAwait(false);
    }
    public async Task<ResponseDto> SubmitJobTypeAsync(JobTypeDto jobTypeDto)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        string sql;

        if (jobTypeDto.JobTypeId <= 0)
        {
            // Insert new job type (assumes JobTypeId is auto-incremented)
            sql = @"
            INSERT INTO JobTypes (TypeName, Description)
            VALUES (@TypeName, @Description)";
        }
        else
        {
            // Update existing job type
            sql = @"
            UPDATE JobTypes
            SET TypeName = @TypeName,
                Description = @Description
            WHERE JobTypeId = @JobTypeId";
        }

        var parameters = new DynamicParameters();
        parameters.Add("@JobTypeId", jobTypeDto.JobTypeId, DbType.Int32);
        parameters.Add("@TypeName", jobTypeDto.TypeName, DbType.String);
        parameters.Add("@Description", jobTypeDto.Description, DbType.String);

        var affectedRows = await connection.ExecuteAsync(sql, parameters).ConfigureAwait(false);

        return new ResponseDto
        {
            IsSuccess = affectedRows > 0,
            Message = affectedRows > 0 ? "Job type submitted successfully." : "Failed to submit job type."
        };
    }



    public async Task<ResponseDto> DeleteJobTypeAsync(int jobTypeId)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        var sql = ""; // write the SQL query to delete a job type by ID

        var parameters = new DynamicParameters();
        parameters.Add("@JobTypeId", jobTypeId, DbType.Int32);

        var affectedRows = await connection.ExecuteAsync(sql, parameters).ConfigureAwait(false);

        if (affectedRows <= 0)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Failed to delete job type."
            };
        }

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "Job type deleted successfully."
        };
    }
}