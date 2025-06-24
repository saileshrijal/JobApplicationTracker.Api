using Dapper;
using JobApplicationTracker.Api.Configuration;
using JobApplicationTracker.Api.Data.Dto;
using JobApplicationTracker.Api.Data.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace JobApplicationTracker.Api.Data.Service;

public class JobsService : IJobsService
{
    public async Task<IEnumerable<JobsDto>> GetAllJobsAsync()
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        var sql = """
              SELECT NotificationId, 
                     UserId, 
                     Title, 
                     Message, 
                     NotificationTypeId,    
                     IsRead, 
                     CreatedAt 
              FROM Notifications
              """;

        return await connection.QueryAsync<JobsDto>(sql).ConfigureAwait(false);
    }
    

    public async Task<JobsDto> GetJobsByIdAsync(int jobId)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        // SQL query to fetch a job by ID
        var sql = """
              SELECT JobId, 
                     CompanyUserId,
                     Title,
                     Description,
                     Requirements,
                     Location,
                     JobTypeId,
                     SalaryMin,
                     SalaryMax,
                     ExperienceLevelId,
                     IsActive,
                     CreatedAt,
                     UpdatedAt,
                     Deadline
              FROM Jobs
              WHERE JobId = @JobId
              """;

        var parameters = new DynamicParameters();
        parameters.Add("@JobId", jobId, DbType.Int32);

        return await connection.QueryFirstOrDefaultAsync<JobsDto>(sql, parameters).ConfigureAwait(false);
    }
    public async Task<ResponseDto> SubmitJobAsync(JobsDto jobsDto)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        string sql;

        if (jobsDto.JobId <= 0)
        {
            // Insert new job
            sql = """
                    INSERT INTO Jobs (CompanyUserId, Title, Description, Requirements, Location, JobTypeId, SalaryMin, SalaryMax, ExperienceLevelId, IsActive, CreatedAt, UpdatedAt, Deadline)
                    VALUES (@CompanyUserId, @Title, @Description, @Requirements, @Location, @JobTypeId, @SalaryMin, @SalaryMax, @ExperienceLevelId, @IsActive, @CreatedAt, @UpdatedAt, @Deadline);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);
                    """;
        }
        else
        {
            // Update existing job
            sql = """
                    UPDATE Jobs
                    SET 
                        CompanyUserId = @CompanyUserId,
                        Title = @Title,
                        Description = @Description,
                        Requirements = @Requirements,
                        Location = @Location,
                        JobTypeId = @JobTypeId,
                        SalaryMin = @SalaryMin,
                        SalaryMax = @SalaryMax,
                        ExperienceLevelId = @ExperienceLevelId,
                        IsActive = @IsActive,
                        UpdatedAt = @UpdatedAt,
                        Deadline = @Deadline
                    WHERE JobId = @JobId
                    """;
        }

            var parameters = new DynamicParameters();
            parameters.Add("@JobId", jobsDto.JobId, DbType.Int32);
            parameters.Add("@CompanyUserId", jobsDto.CompanyUserId, DbType.Int32);
            parameters.Add("@Title", jobsDto.Title, DbType.String);
            parameters.Add("@Description", jobsDto.Description, DbType.String);
            parameters.Add("@Requirements", jobsDto.Requirements, DbType.String);
            parameters.Add("@Location", jobsDto.Location, DbType.String);
            parameters.Add("@JobTypeId", jobsDto.JobTypeId, DbType.Int32);
            parameters.Add("@SalaryMin", jobsDto.SalaryMin, DbType.Decimal);
            parameters.Add("@SalaryMax", jobsDto.SalaryMax, DbType.Decimal);
            parameters.Add("@ExperienceLevelId", jobsDto.ExperienceLevelId, DbType.Int32);
            parameters.Add("@IsActive", jobsDto.IsActive, DbType.Boolean);
            parameters.Add("@CreatedAt", jobsDto.CreatedAt, DbType.DateTime);
            parameters.Add("@UpdatedAt", DateTime.UtcNow, DbType.DateTime);
            parameters.Add("@Deadline", jobsDto.Deadline, DbType.DateTime);

            var affectedRows = 0;

        if (jobsDto.JobId <= 0)
        {
            // Insert operation
            var newId = await connection.QuerySingleAsync<int>(sql, parameters).ConfigureAwait(false);
            affectedRows = newId > 0 ? 1 : 0;
            jobsDto.JobId = newId; // Set the ID for the newly inserted record
        }
        else
        {
            // Update operation
            affectedRows = await connection.ExecuteAsync(sql, parameters).ConfigureAwait(false);
        }

        return new ResponseDto
        {
            IsSuccess = affectedRows > 0,
            Message = affectedRows > 0 ? "Job submitted successfully." : "Failed to submit job."
        };
    }



    public async Task<ResponseDto> DeleteJobAsync(int jobId)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        // SQL query to delete a job by ID
        var sql = """DELETE FROM Jobs WHERE JobId = @JobId""";

        var parameters = new DynamicParameters();
        parameters.Add("@JobId", jobId, DbType.Int32);

        var affectedRows = await connection.ExecuteAsync(sql, parameters).ConfigureAwait(false);

        if (affectedRows <= 0)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Failed to delete job."
            };
        }

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "Job deleted successfully."
        };
    }
}