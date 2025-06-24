using Dapper;
using JobApplicationTracker.Api.Configuration;
using JobApplicationTracker.Api.Data.Dto;
using JobApplicationTracker.Api.Data.Interface;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace JobApplicationTracker.Api.Data.Service;

public class UserTypesService : IUsersTypeService
{
    public async Task<IEnumerable<UserTypesDto>> GetAllUserTypesAsync()
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        var sql = """
                  SELECT UserTypeId, 
                         TypeName, 
                         Description 
                  FROM UserTypes
                  """;

        return await connection.QueryAsync<UserTypesDto>(sql).ConfigureAwait(false);
    }

    public async Task<UserTypesDto> GetUserTypesByIdAsync(int userTypesId)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        // write the SQL query to fetch a user type by ID
        var sql = """
                  SELECT UserTypeId, 
                         TypeName, 
                         Description 
                  FROM UserTypes
                  WHERE UserTypesId = @UserTypesId
                  """;


        var parameters = new DynamicParameters();
        parameters.Add("@UserTypeId", userTypesId, DbType.Int32);

        return await connection.QueryFirstOrDefaultAsync<UserTypesDto>(sql, parameters).ConfigureAwait(false);
    }
    public async Task<ResponseDto> SubmitUserTypesAsync(UserTypesDto userTypesDto)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        string sql;

        if (userTypesDto.UserTypeId <= 0)
        {
            // Insert new skill (assumes user type id is auto-incremented)
            sql = """
            INSERT INTO UserTypes (TypeName, Description)
            VALUES (@TypeName, @Description)
            """;
        }
        else
        {
            // Update existing user type
            sql = """
            UPDATE UserTypes
            SET TypeName = TypeName,
                Description= Description
            WHERE UserTypeId = UserTypeId
            """;
        }

        var parameters = new DynamicParameters();
        parameters.Add("UserTypeId", userTypesDto.UserTypeId, DbType.Int32);
        parameters.Add("TypeName", userTypesDto.TypeName, DbType.String);
        parameters.Add("Description", userTypesDto.Description, DbType.String);

        var affectedRows = await connection.ExecuteAsync(sql, parameters).ConfigureAwait(false);

        return new ResponseDto
        {
            IsSuccess = affectedRows > 0,
            Message = affectedRows > 0 ? "Usertype submitted successfully." : "Failed to submit user type."
        };
    }



    public async Task<ResponseDto> DeleteUserTypesAsync(int userTypesId)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();
        // write the SQL query to delete user type by ID

        var sql = """
                DELETE FROM UserTypes 
                WHERE UserTypeId = UserTypeId
                """;

        var parameters = new DynamicParameters();
        parameters.Add("@UserTypeId", userTypesId, DbType.Int32);

        var affectedRows = await connection.ExecuteAsync(sql, parameters).ConfigureAwait(false);

        if (affectedRows <= 0)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Failed to delete User Types."
            };
        }

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "User types deleted successfully."
        };
    }
}
