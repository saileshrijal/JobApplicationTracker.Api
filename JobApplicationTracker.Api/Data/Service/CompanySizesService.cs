using Dapper;
using JobApplicationTracker.Api.Configuration;
using JobApplicationTracker.Api.Data.Dto;
using JobApplicationTracker.Api.Data.Interface;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace JobApplicationTracker.Api.Data.Service;

public class CompanySizeService : ICompanySizesService
{
    public async Task<IEnumerable<CompanySizeDto>> GetAllCompanySizeAsync()
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        var sql = """
                  SELECT CompanySizeId, 
                         SizeName,
                         Description,
                         MinEmployees,
                         MaxEmployees
                  FROM CompanySizes
                  """;


        return await connection.QueryAsync<CompanySizeDto>(sql).ConfigureAwait(false);
    }

    public async Task<CompanySizeDto> GetCompanySizeByIdAsync(int companySizeId)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();
        // write the SQL query to fetch a company size by ID
        var sql = """
                  SELECT CompanySizeId, 
                         SizeName,
                         Description,
                         MinEmployees,
                         MaxEmployees
                  FROM CompanySizes
                  WHERE CompanyId = CompanyId
                  """;

        var parameters = new DynamicParameters();
        parameters.Add("@CompanySizeId", companySizeId, DbType.Int32);

        return await connection.QueryFirstOrDefaultAsync<CompanySizeDto>(sql, parameters).ConfigureAwait(false);
    }



    public async Task<ResponseDto> SubmitCompanySizeAsync(CompanySizeDto companySizeDto)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        string sql;

        if (companySizeDto.CompanySizeId <= 0)
        {
            // Insert new company size (assumes CompanySizeId is auto-incremented)
            sql = """
                    INSERT INTO CompanySizes (SizeName, Description, MinEmployees, MaxEmployees)
                    VALUES (@SizeName, @Description, @MinEmployees, @MaxEmployees);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);
                    """;
        }
        else
        {
            // Update existing company size
            sql = """
                    UPDATE CompanySizes
                    SET 
                        SizeName = @SizeName,
                        Description = @Description,
                        MinEmployees = @MinEmployees,
                        MaxEmployees = @MaxEmployees
                    WHERE CompanySizeId = @CompanySizeId
                    """;
        }

        var parameters = new DynamicParameters();
        parameters.Add("@CompanySizeId", companySizeDto.CompanySizeId, DbType.Int32);
        parameters.Add("@SizeName", companySizeDto.SizeName, DbType.String);
        parameters.Add("@Description", companySizeDto.Description, DbType.String);
        parameters.Add("@MinEmployees", companySizeDto.MinEmployees, DbType.Int32);
        parameters.Add("@MaxEmployees", companySizeDto.MaxEmployees, DbType.Int32);

        var affectedRows = 0;

        if (companySizeDto.CompanySizeId <= 0)
        {
            // Insert operation
            var newId = await connection.QuerySingleAsync<int>(sql, parameters).ConfigureAwait(false);
            affectedRows = newId > 0 ? 1 : 0;
            companySizeDto.CompanySizeId = newId; // Set the ID for the newly inserted record
        }
        else
        {
            // Update operation
            affectedRows = await connection.ExecuteAsync(sql, parameters).ConfigureAwait(false);
        }

        return new ResponseDto
        {
            IsSuccess = affectedRows > 0,
            Message = affectedRows > 0 ? "Company size submitted successfully." : "Failed to submit company size."
        };
    }


    public async Task<ResponseDto> DeleteCompanySizeAsync(int companySizeId)
    {
        await using var connection = new SqlConnection(JobApplicationTrackerConfig.ConnectionString);
        await connection.OpenAsync();

        // SQL query to delete a company size by ID
        var sql = """DELETE FROM CompanySizes WHERE CompanySizeId = @CompanySizeId""";

        var parameters = new DynamicParameters();
        parameters.Add("@CompanySizeId", companySizeId, DbType.Int32);

        var affectedRows = await connection.ExecuteAsync(sql, parameters).ConfigureAwait(false);

        if (affectedRows <= 0)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Failed to delete company size."
            };
        }

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "Company size deleted successfully."
        };
    }
}