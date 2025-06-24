using Dapper;
using JobApplicationTracker.Api.Configuration;
using JobApplicationTracker.Api.Data.Dto;
using JobApplicationTracker.Api.Data.Interface;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace JobApplicationTracker.Api.Data.Service;

public class SkillsService : ISkillsService
{
    public async Task<IEnumerable<SkillsDto>> GetAllSkillsAsync()
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        var sql = """
                  SELECT SkillId, 
                         SkillName, 
                         Category 
                  FROM Skills
                  """;

        return await connection.QueryAsync<SkillsDto>(sql).ConfigureAwait(false);
    }

    public async Task<SkillsDto> GetSkillsByIdAsync(int skillId)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();
        // write the SQL query to fetch a skill by ID
        var sql = """
                  SELECT SkillId, 
                         SkillName, 
                         Category 
                  FROM Skills
                  WHERE SkillId = @SkillId
                  """;
        

        var parameters = new DynamicParameters();
        parameters.Add("@SkillId", skillId, DbType.Int32);

        return await connection.QueryFirstOrDefaultAsync<SkillsDto>(sql, parameters).ConfigureAwait(false);
    }
    public async Task<ResponseDto> SubmitSkillsAsync(SkillsDto skillsDto)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        string sql;

        if (skillsDto.SkillId <= 0)
        {
            // Insert new skill (assumes JobTypeId is auto-incremented)
            sql = """
            INSERT INTO Skills (SkillName, Category)
            VALUES (@SkillName, @Category)
            """;
        }
        else
        {
            // Update existing skill
            sql = """
            UPDATE Skills
            SET SkillName = SkillName,
                Category = Category
            WHERE SkillId = SkillId
            """;
        }

        var parameters = new DynamicParameters();
        parameters.Add("SkillId", skillsDto.SkillId, DbType.Int32);
        parameters.Add("SkillName", skillsDto.SkillName, DbType.String);
        parameters.Add("Description", skillsDto.Category, DbType.String);

        var affectedRows = await connection.ExecuteAsync(sql, parameters).ConfigureAwait(false);

        return new ResponseDto
        {
            IsSuccess = affectedRows > 0,
            Message = affectedRows > 0 ? "Job type submitted successfully." : "Failed to submit job type."
        };
    }



    public async Task<ResponseDto> DeleteSkillsAsync(int skillId)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();
        // write the SQL query to delete askill by ID
        var sql = """DELETE FROM Skills WHERE SkillId = SkillId""";

        var parameters = new DynamicParameters();
        parameters.Add("@SkillId", skillId, DbType.Int32);

        var affectedRows = await connection.ExecuteAsync(sql, parameters).ConfigureAwait(false);

        if (affectedRows <= 0)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Failed to delete Skill."
            };
        }

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "Skill deleted successfully."
        };
    }
}
