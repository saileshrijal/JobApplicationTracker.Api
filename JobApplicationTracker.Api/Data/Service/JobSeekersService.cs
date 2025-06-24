using Dapper;
using JobApplicationTracker.Api.Configuration;
using JobApplicationTracker.Api.Data.Dto;
using JobApplicationTracker.Api.Data.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Numerics;

namespace JobApplicationTracker.Api.Data.Service;

public class JobSeekerService : IJobSeekersService
{
    public async Task<IEnumerable<JobSeekersDto>> GetAllJobSeekersAsync()
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        var sql = """
                  SELECT JobSeekerId, 
                         FirstName,
                         LastName,
                         Phone,
                         Location,
                         DateOfBirth,
                         ProfilePicture,
                         Resume,
                         Bio,
                         CreatedAt,
                         UpdatedAt

                  FROM JobSeekers
                  """;




        return await connection.QueryAsync<JobSeekersDto>(sql).ConfigureAwait(false);
    }

    public async Task<JobSeekersDto> GetJobSeekersByIdAsync(int jobSeekerId)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        // SQL query to fetch a job seeker by ID
        var sql = """
              SELECT JobSeekerId, 
                     FirstName,
                     LastName,
                     Phone,
                     Location,
                     DateOfBirth,
                     ProfilePicture,
                     Resume,
                     Bio,
                     CreatedAt,
                     UpdatedAt
              FROM JobSeekers
              WHERE JobSeekerId = @JobSeekerId
              """;

        var parameters = new DynamicParameters();
        parameters.Add("@JobSeekerId", jobSeekerId, DbType.Int32);

        return await connection.QueryFirstOrDefaultAsync<JobSeekersDto>(sql, parameters).ConfigureAwait(false);
    }
    public async Task<ResponseDto> SubmitJobSeekersAsync(JobSeekersDto jobSeekerDto)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        string sql;

        if (jobSeekerDto.JobSeekerId <= 0)
        {
            // Insert new job seeker (assumes JobSeekerId is auto-incremented)
            sql = """
        INSERT INTO JobSeekers (FirstName, LastName, Phone, Location, DateOfBirth, ProfilePicture, Resume, Bio, CreatedAt, UpdatedAt)
        VALUES (@FirstName, @LastName, @Phone, @Location, @DateOfBirth, @ProfilePicture, @Resume, @Bio, @CreatedAt, @UpdatedAt);
        SELECT CAST(SCOPE_IDENTITY() AS INT);
        """;
        }
        else
        {
            // Update existing job seeker
            sql = """
        UPDATE JobSeekers
        SET 
            FirstName = @FirstName,
            LastName = @LastName,
            Phone = @Phone,
            Location = @Location,
            DateOfBirth = @DateOfBirth,
            ProfilePicture = @ProfilePicture,
            Resume = @Resume,
            Bio = @Bio,
            UpdatedAt = @UpdatedAt
        WHERE JobSeekerId = @JobSeekerId
        """;
        }

        var parameters = new DynamicParameters();
        parameters.Add("@JobSeekerId", jobSeekerDto.JobSeekerId, DbType.Int32);
        parameters.Add("@FirstName", jobSeekerDto.FirstName, DbType.String);
        parameters.Add("@LastName", jobSeekerDto.LastName, DbType.String);
        parameters.Add("@Phone", jobSeekerDto.Phone, DbType.String);
        parameters.Add("@Location", jobSeekerDto.Location, DbType.String);
        parameters.Add("@DateOfBirth", jobSeekerDto.DateOfBirth, DbType.DateTime);
        parameters.Add("@ProfilePicture", jobSeekerDto.ProfilePicture, DbType.String);
        parameters.Add("@Resume", jobSeekerDto.Resume, DbType.String);
        parameters.Add("@Bio", jobSeekerDto.Bio, DbType.String);
        parameters.Add("@CreatedAt", jobSeekerDto.CreatedAt, DbType.DateTime);
        parameters.Add("@UpdatedAt", DateTime.UtcNow, DbType.DateTime);

        var affectedRows = await connection.ExecuteAsync(sql, parameters).ConfigureAwait(false);

        return new ResponseDto
        {
            IsSuccess = affectedRows > 0,
            Message = affectedRows > 0 ? "Job type submitted successfully." : "Failed to submit job type."
        };
    }
    public async Task<ResponseDto> DeleteJobSeekersAsync(int jobSeekerId)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        // SQL query to delete a job seeker by ID
        var sql = """DELETE FROM JobSeekers WHERE JobSeekerId = @JobSeekerId""";

        var parameters = new DynamicParameters();
        parameters.Add("@JobSeekerId", jobSeekerId, DbType.Int32);

        var affectedRows = await connection.ExecuteAsync(sql, parameters).ConfigureAwait(false);

        if (affectedRows <= 0)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Failed to delete job seeker."
            };
        }

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "Job seeker deleted successfully."
        };
    }
}