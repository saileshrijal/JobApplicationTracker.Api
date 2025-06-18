using System.Data;
using Dapper;
using JobApplicationTracker.Api.Configuration;
using JobApplicationTracker.Api.Data.Dto;
using JobApplicationTracker.Api.Data.Interface;
using Microsoft.Data.SqlClient;

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

        var sql = ""; // write the SQL query to fetch a job type by ID

        var parameters = new DynamicParameters();
        parameters.Add("@JobTypeId", jobTypeId, DbType.Int32);

        return await connection.QueryFirstOrDefaultAsync<JobTypeDto>(sql, parameters).ConfigureAwait(false);
    }

    public async Task<ResponseDto> SubmitJobTypeAsync(JobTypeDto jobTypeDto)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        var sql = jobTypeDto.JobTypeId <= 0
            ? "" // write the SQL query to insert a new job type
            : ""; // write the SQL query to update a job type

        var parameters = new DynamicParameters();
        parameters.Add("@JobTypeId", jobTypeDto.JobTypeId, DbType.Int32);
        parameters.Add("@TypeName", jobTypeDto.TypeName, DbType.String);
        parameters.Add("@Description", jobTypeDto.Description, DbType.String);

        var affectedRows = await connection.ExecuteAsync(sql, parameters).ConfigureAwait(false);

        if (affectedRows <= 0)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Failed to submit job type."
            };
        }

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "Job type submitted successfully."
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