using Dapper;
using JobApplicationTracker.Api.Configuration;
using JobApplicationTracker.Api.Data.Dto;
using JobApplicationTracker.Api.Data.Interface;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace JobApplicationTracker.Api.Data.Service;

public class ApplicationsService : IJobApplicationService
{
    public async Task<IEnumerable<JobApplicationDto>> GetAllJobApplicationAsync()
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        var sql = """
              SELECT JobApplicationId, 
                     JobId, 
                     JobSeekerId, 
                     CoverLetter, 
                     StatusId, 
                     AppliedAt, 
                     UpdatedAt
              FROM JobApplications
              """;

        return await connection.QueryAsync<JobApplicationDto>(sql).ConfigureAwait(false);
    }

    public async Task<JobApplicationDto> GetJobApplicationByIdAsync(int jobApplicationId)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        // write the SQL query to fetch a job application by ID
        var sql = """
                  SELECT   JobApplicationId,
                           JobId,
                           JobSeekerId,
                           CoverLetter,
                           StatusId,
                           AppliedAt,
                           UpdatedAt

                       
                  FROM JobApplications

                  WHERE ApplicationId = @JobApplicationId
                  """;
       
        var parameters = new DynamicParameters();
        parameters.Add("@JobApplicationId", jobApplicationId, DbType.Int32);

        return await connection.QueryFirstOrDefaultAsync<JobApplicationDto>(sql, parameters).ConfigureAwait(false);
    }
    public async Task<ResponseDto> SubmitJobApplicationAsync(JobApplicationDto jobApplicationDto)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        string sql;

        if (jobApplicationDto.JobApplicationId <= 0)
        {
            // Insert new job application (assumes ApplicationId is auto-incremented)
            sql = """
                    INSERT INTO Applications (JobId, JobSeekerUserId, CoverLetter, StatusId, AppliedAt, UpdatedAt)

                    VALUES (@JobId, @JobSeekerUserId, @CoverLetter, @StatusId, @AppliedAt, @UpdatedAt);

                    SELECT CAST(SCOPE_IDENTITY() AS INT);

                    """;
        }
        else
        {
            // Update existing job application
            sql = """
                    UPDATE Applications
                    SET 
                        JobId = @JobId,
                        JobSeekerId = @JobSeekerId,
                        CoverLetter = @CoverLetter,
                        StatusId = @StatusId,
                        UpdatedAt = @UpdatedAt
                    WHERE ApplicationId = @ApplicationId
                    """;
        }

        var parameters = new DynamicParameters();
        parameters.Add("@JobApplicationId", jobApplicationDto.JobApplicationId, DbType.Int32);
        parameters.Add("@JobId", jobApplicationDto.JobId, DbType.Int32);
        parameters.Add("@JobSeekerId", jobApplicationDto.JobSeekerId, DbType.Int32);
        parameters.Add("@CoverLetter", jobApplicationDto.CoverLetter, DbType.String);
        parameters.Add("@StatusId", jobApplicationDto.StatusId, DbType.Int32);
        parameters.Add("@AppliedAt", DateTime.UtcNow, DbType.DateTime);

        // Set UpdatedAt for both insert and update
        parameters.Add("@UpdatedAt", DateTime.UtcNow, DbType.DateTime);

        var affectedRows = 0;

        if (jobApplicationDto.JobApplicationId <= 0)
        {
            var newId = await connection.QuerySingleAsync<int>(sql, parameters).ConfigureAwait(false);
            affectedRows = newId > 0 ? 1 : 0;
            jobApplicationDto.JobApplicationId = newId; // Set the ID for the newly inserted record
        }
        else
        {
            affectedRows = await connection.ExecuteAsync(sql, parameters).ConfigureAwait(false);
        }

        return new ResponseDto
        {
            IsSuccess = affectedRows > 0,
            Message = affectedRows > 0 ? "Job application submitted successfully." : "Failed to submit job application.",

        };
    }



    public async Task<ResponseDto> DeleteJobApplicationAsync(int jobApplicationId)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        // write the SQL query to delete a job application by ID
        var sql = """DELETE FROM JobApplications WHERE JobApplicationId = JobApplicationId""";

        var parameters = new DynamicParameters();
        parameters.Add("@JobApplicationId", jobApplicationId, DbType.Int32);

        var affectedRows = await connection.ExecuteAsync(sql, parameters).ConfigureAwait(false);

        if (affectedRows <= 0)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Failed to delete job application."
            };
        }

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "Job application deleted successfully."
        };
    }
}
