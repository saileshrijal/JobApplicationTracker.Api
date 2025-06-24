using Dapper;
using JobApplicationTracker.Api.Configuration;
using JobApplicationTracker.Api.Data.Dto;
using JobApplicationTracker.Api.Data.Interface;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace JobApplicationTracker.Api.Data.Service;

public class ApplicationStatusService : IJobApplicationStatusService
{
    public async Task<IEnumerable<JobApplicationStatusDto>> GetAllJobApplicationStatusAsync()
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        var sql = """
                  SELECT StatusId, 
                         StatusName, 
                         Description ,
                         IsActive

                  FROM ApplicationStatus
                  """;

        return await connection.QueryAsync<JobApplicationStatusDto>(sql).ConfigureAwait(false);
    }

    public async Task<JobApplicationStatusDto> GetJobApplicationStatusByIdAsync(int jobApplicationStatusId)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();
        // write the SQL query to fetch a job application Status by ID
        var sql = """
                  SELECT JobTypeId, 
                         TypeName, 
                         Description 
                  FROM JobTypes
                  WHERE JobTypeId = ?
                  """;

        var parameters = new DynamicParameters();
        parameters.Add("@JobApplicationStatusId", jobApplicationStatusId, DbType.Int32);

        return await connection.QueryFirstOrDefaultAsync<JobApplicationStatusDto>(sql, parameters).ConfigureAwait(false);
    }
    public async Task<ResponseDto> SubmitJobApplicationStatusAsync(JobApplicationStatusDto jobApplicationStatusDto)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        string sql;

        if (jobApplicationStatusDto.StatusId <= 0)
        {
            // Insert new job application status (assumes StatusId is auto-incremented)
            sql = """
                    INSERT INTO ApplicationStatus (StatusName, Description, IsActive)
                    VALUES (@StatusName, @Description, @IsActive);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);
                    """;
        }
        else
        {
            // Update existing job application status
            sql = """
                    UPDATE ApplicationStatus
                    SET 
                        StatusName = @StatusName,
                        Description = @Description,
                        IsActive = @IsActive
                    WHERE StatusId = @StatusId
                    """;
        }

        var parameters = new DynamicParameters();
        parameters.Add("@StatusId", jobApplicationStatusDto.StatusId, DbType.Int32);
        parameters.Add("@StatusName", jobApplicationStatusDto.StatusName, DbType.String);
        parameters.Add("@Description", jobApplicationStatusDto.Description, DbType.String);
        parameters.Add("@IsActive", jobApplicationStatusDto.IsActive, DbType.Boolean);

        var affectedRows = 0;

        if (jobApplicationStatusDto.StatusId <= 0)
        {
            var newId = await connection.QuerySingleAsync<int>(sql, parameters).ConfigureAwait(false);
            affectedRows = newId > 0 ? 1 : 0;
            jobApplicationStatusDto.StatusId = newId; // Set the ID for the newly inserted record
        }
        else
        {
            affectedRows = await connection.ExecuteAsync(sql, parameters).ConfigureAwait(false);
        }

        return new ResponseDto
        {
            IsSuccess = affectedRows > 0,
            Message = affectedRows > 0 ? "Job application status submitted successfully." : "Failed to submit job application status.",

        };
    }


    public async Task<ResponseDto> DeleteJobApplicationStatusAsync(int statusId)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        // SQL query to delete a job application status by ID
        var sql = """DELETE FROM ApplicationStatus WHERE StatusId = @StatusId""";

        var parameters = new DynamicParameters();
        parameters.Add("@StatusId", statusId, DbType.Int32);

        var affectedRows = await connection.ExecuteAsync(sql, parameters).ConfigureAwait(false);

        if (affectedRows <= 0)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Failed to delete job application status."
            };
        }

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "Job application status deleted successfully."
        };
    }
}