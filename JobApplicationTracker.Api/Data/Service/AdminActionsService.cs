
using Dapper;
using JobApplicationTracker.Api.Configuration;
using JobApplicationTracker.Api.Data.Dto;
using JobApplicationTracker.Api.Data.Interface;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace JobApplicationTracker.Api.Data.Service;

public class AdminActionService : IAdminActionService
{
    public async Task<IEnumerable<AdminActionsDto>> GetAllJobTypesAsync()
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        var sql = """
                  SELECT ActionId, 
                         ActionName, 
                         Description 
                  FROM AdminAction
                  """;

        return await connection.QueryAsync<AdminActionsDto>(sql).ConfigureAwait(false);
    }

    public async Task<AdminActionsDto> GetAdminActionByIdAsync(int adminActionsId)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();
        // write the SQL query to fetch an admin actions by ID
        var sql = """
                  SELECT ActionId,
                     ActionName,
                     Description,
                     
                  FROM AdminActions
                  WHERE ActionId = @ActionId
                  """;

        var parameters = new DynamicParameters();
        parameters.Add("@AdminActionsId", adminActionsId, DbType.Int32);

        return await connection.QueryFirstOrDefaultAsync<AdminActionsDto>(sql, parameters).ConfigureAwait(false);
    }
    public async Task<ResponseDto> SubmitAdminActionsAsync(AdminActionsDto adminActionsDto)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        string sql;

        if (adminActionsDto.ActionId <= 0)
        {
            // Insert new job type (assumes admin action Id is auto-incremented)
            sql = """
            INSERT INTO AdminActions (ActionName, Description)
            VALUES (@ActionName, @Description)
            """;
        }
        else
        {
            // Update existing admin action
            sql = """
            UPDATE AdminActions
            SET ActionName = ActionName,
                Description = Description
            WHERE ActionId = ActionId
            """;
        }

        var parameters = new DynamicParameters();
        parameters.Add("ActionId", adminActionsDto.ActionId, DbType.Int32);
        parameters.Add("ActionName", adminActionsDto.ActionName, DbType.String);
        parameters.Add("Description", adminActionsDto.Description, DbType.String);

        var affectedRows = await connection.ExecuteAsync(sql, parameters).ConfigureAwait(false);

        return new ResponseDto
        {
            IsSuccess = affectedRows > 0,
            Message = affectedRows > 0 ? "Admin's action submitted successfully." : "Failed to submit Admin's actions."
        };
    }



    public async Task<ResponseDto> DeleteAsdminActionAsync(int adminActionsId)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();
        // write the SQL query to delete a admin action by ID
        var sql = """
                   DELETE FROM AdminActions 
                   WHERE ActionId = ActionId
                  """;

        var parameters = new DynamicParameters();
        parameters.Add("@AdminActionId", adminActionsId, DbType.Int32);

        var affectedRows = await connection.ExecuteAsync(sql, parameters).ConfigureAwait(false);

        if (affectedRows <= 0)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Failed to delete admin actions."
            };
        }

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "Admin actions deleted successfully."
        };
    }
}
