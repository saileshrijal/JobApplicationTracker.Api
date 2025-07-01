using Dapper;
using JobApplicationTracker.Api.Configuration;
using JobApplicationTracker.Api.Data.Dto;
using JobApplicationTracker.Api.Data.Interface;
using Microsoft.Data.SqlClient;
using System.Data;

namespace JobApplicationTracker.Api.Data.Service;

public class UsersService : IUsersService
{
    public async Task<IEnumerable<UsersDto>> GetAllUsersAsync()
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        var sql = """
                  SELECT UserId, 
                         Email, 
                         PasswordHash
                         UserTypeId
                         IsAdmin
                         CreatedAt
                         UpdatedAt
                         IsActive
                  FROM Users
                  """; 

        return await connection.QueryAsync<UsersDto>(sql).ConfigureAwait(false);
    }

    public async Task<UsersDto> GetUsersByIdAsync(int usersId)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();
        // write the SQL query to fetch a Users by ID
        var sql = """
                    SELECT * 
                    FROM Users 
                    WHERE UserId = @UserId
                    """;

        var parameters = new DynamicParameters();
        parameters.Add("@UsersId", usersId, DbType.Int32);

        return await connection.QueryFirstOrDefaultAsync<UsersDto>(sql, parameters).ConfigureAwait(false);
    }
    public async Task<ResponseDto> SubmitUsersAsync(UsersDto userDto)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        string sql;
        //Encryption of PasswordHash is assumed to be handled before this method is called

        if (userDto.UserId <= 0)
        {
            // Insert new users (assumes userId is auto-incremented)
            sql = """
                  INSERT INTO Users (Email, PasswordHash, UserType, CreatedAt, UpdatedAt, IsActive)
                  VALUES (@Email, @PasswordHash, @UserType, @CreatedAt, @UpdatedAt, @IsActive)
                  """;
        }
        else
        {
            // Update existing user
            sql = """
                    UPDATE Users
                    SET Email = @Email,
                        PasswordHash = @PasswordHash,
                        UserType = @UserType,
                        IsAdmin = @IsAdmin,
                        UpdatedAt = @UpdatedAt,
                        IsActive = @IsActive
                    WHERE UserId = @UserId
                    """;
        }

        var parameters = new DynamicParameters();
        parameters.Add("UserId", userDto.UserId, DbType.Int32);
        parameters.Add("Email", userDto.Email, DbType.String);
        parameters.Add("PasswordHash", userDto.PasswordHash, DbType.String);
        parameters.Add("UserType", userDto.UserType, DbType.String);
        parameters.Add("IsAdmin", userDto.IsAdmin, DbType.Boolean);
        parameters.Add("UpdatedAt", DateTime.UtcNow, DbType.DateTime); 
        parameters.Add("CreatedAt", DateTime.UtcNow, DbType.DateTime);
        parameters.Add("IsActive", userDto.IsActive, DbType.Boolean);

        var affectedRows = await connection.ExecuteAsync(sql, parameters).ConfigureAwait(false);

        return new ResponseDto
        {
            IsSuccess = affectedRows > 0,
            Message = affectedRows > 0 ? "User updated successfully." : "Failed to update user."
        };
    }



    public async Task<ResponseDto> DeleteUsersAsync(int userId)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        // Write the SQL query to delete a user by ID
        var sql = """DELETE FROM Users WHERE UserId = @UserId""";

        var parameters = new DynamicParameters();
        parameters.Add("@UserId", userId, DbType.Int32);

        var affectedRows = await connection.ExecuteAsync(sql, parameters).ConfigureAwait(false);

        if (affectedRows <= 0)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Failed to delete user."
            };
        }

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "User deleted successfully."
        };
    }

    public async Task<ResponseDto> CreateUserAsync(SignupDto credentials)
    {
        var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        var query = 
            @" 
                INSERT INTO Users (Email, PasswordHash, UserTypeId, IsAdmin, CreatedAt, UpdatedAt, IsActive)
                VALUES(@Email, @PasswordHash, @UserTypeId, @IsAdmin, @CreatedAt, @UpdatedAt, @IsActive)
            ";


        var parameters = new DynamicParameters();
        //parameters.Add("UserId", userDto.UserId, DbType.Int32);
        parameters.Add("Email", credentials.Email, DbType.String);
        parameters.Add("PasswordHash", credentials.PasswordHash, DbType.String);
        //parameters.Add("UserTypeId", userDto.UserTypeId, DbType.Int32);
        //parameters.Add("IsAdmin", userDto.IsAdmin, DbType.Boolean);
        //parameters.Add("UpdatedAt", DateTime.UtcNow, DbType.DateTime);
        //parameters.Add("IsActive", userDto.IsActive, DbType.Boolean);

        var affectedRows = await connection.ExecuteAsync(query, parameters).ConfigureAwait(false);

        if (affectedRows <= 0)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Failed to create user."
            };
        }

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "User created successfully."
        };
    }

    public async Task<bool> DoesEmailExists(string email)
    {
        var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        var query = """SELECT 1 FROM Users WHERE LOWER(Email) = LOWER(@Email)""";

        var parameters = new DynamicParameters();
        parameters.Add("@Email",email, DbType.String);

        var result = await connection.ExecuteScalarAsync<int?>(query,parameters).ConfigureAwait(false);

        return result.HasValue;
    }

    public async Task<UsersDto?> GetUserByEmail(string email)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        var query = """SELECT * FROM Users WHERE Email = @Email""";

        var parameters = new DynamicParameters();
        parameters.Add("@Email", email, DbType.String);

        return await connection.QueryFirstOrDefaultAsync<UsersDto>(query,parameters).ConfigureAwait(false);
    }
}
   

