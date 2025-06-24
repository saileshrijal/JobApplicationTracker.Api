using Dapper;
using JobApplicationTracker.Api.Configuration;
using JobApplicationTracker.Api.Data.Dto;
using JobApplicationTracker.Api.Data.Interface;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace JobApplicationTracker.Api.Data.Service;

public class NotificationTypesService : INotificationsTypesService
{
    public async Task<IEnumerable<NotificationTypesDto>> GetAllNotificationTypesAsync()
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        var sql = """
                  SELECT NotificationTypeId, 
                         TypeName, 
                         Description 
                         Priority
                  FROM NotificationTypes
                  """;

        return await connection.QueryAsync< NotificationTypesDto >(sql).ConfigureAwait(false);
    }

    public async Task<NotificationTypesDto> GetNotificationTypesByIdAsync(int notificationTypeId)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        // SQL query to fetch a notification type by ID
        var sql = """
              SELECT NotificationTypeId, 
                     TypeName, 
                     Description, 
                     Priority
              FROM NotificationTypes
              WHERE NotificationTypeId = @NotificationTypeId
              """;

        var parameters = new DynamicParameters();
        parameters.Add("@NotificationTypeId", notificationTypeId, DbType.Int32);

        return await connection.QueryFirstOrDefaultAsync<NotificationTypesDto>(sql, parameters).ConfigureAwait(false);
    }
    public async Task<ResponseDto> SubmitNotificationTypesAsync(NotificationTypesDto notificationTypesDto)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        string sql;

        if (notificationTypesDto.NotificationTypeId <= 0)
        {
            // Insert new notification type (assumes NotificationTypeId is auto-incremented)
            sql = """
        INSERT INTO NotificationTypes (TypeName, Description, Priority)
        VALUES (@TypeName, @Description, @Priority);
        """;
        }
        else
        {
            // Update existing notification type
            sql = """
        UPDATE NotificationTypes
        SET 
            TypeName = @TypeName,
            Description = @Description,
            Priority = @Priority
        WHERE NotificationTypeId = @NotificationTypeId;
        """;
        }

        var parameters = new DynamicParameters();
        parameters.Add("@NotificationTypeId", notificationTypesDto.NotificationTypeId, DbType.Int32);
        parameters.Add("@TypeName", notificationTypesDto.TypeName, DbType.String);
        parameters.Add("@Description", notificationTypesDto.Description, DbType.String);
        parameters.Add("@Priority", notificationTypesDto.Priority, DbType.Int32); // Assuming Priority is an integer

        var affectedRows = await connection.ExecuteAsync(sql, parameters).ConfigureAwait(false);

        return new ResponseDto
        {
            IsSuccess = affectedRows > 0,
            Message = affectedRows > 0 ? "Notification type submitted successfully." : "Failed to submit notification type."
        };
    }
    public async Task<ResponseDto> DeleteNotificationTypesAsync(int notificationTypesId)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        // Check referential integrity 
        var refCheckSql = "SELECT COUNT(1) FROM SomeRelatedTable WHERE NotificationTypeId = @NotificationTypeId"; // Adjust table name as needed
        var hasDependencies = await connection.ExecuteScalarAsync<int>(refCheckSql, new { NotificationTypeId = notificationTypesId });

        if (hasDependencies > 0)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Cannot delete notification type because it has associated records."
            };
        }

        // Delete the notification type
        var deleteSql = "DELETE FROM NotificationTypes WHERE NotificationTypeId = @NotificationTypeId";

        var parameters = new DynamicParameters();
        parameters.Add("@NotificationTypeId", notificationTypesId, DbType.Int32);

        var affectedRows = await connection.ExecuteAsync(deleteSql, parameters).ConfigureAwait(false);

        if (affectedRows <= 0)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Notification type not found or could not be deleted."
            };
        }

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "Notification type deleted successfully."
        };
    }
}
