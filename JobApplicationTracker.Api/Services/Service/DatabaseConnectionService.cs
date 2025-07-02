using JobApplicationTracker.Api.Configuration;
using JobApplicationTracker.Api.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace JobApplicationTracker.Api.Services.Service
{
    public class DatabaseConnectionService : IDatabaseConnectionService
    {
        private readonly DatabaseConfig _config;

        public DatabaseConnectionService(IOptions<DatabaseConfig> config)
        {
            _config = config.Value;
        }
        public async Task CloseDatabaseConnectionAsync(SqlConnection connection)
        {
            if (connection != null && connection.State != System.Data.ConnectionState.Closed)
            {
                await connection.CloseAsync();
            }
        }

        public async  Task<SqlConnection> GetDatabaseConnectionAsync()
        {
			try
			{
				var connection = new SqlConnection(_config.ConnectionString);
				await connection.OpenAsync();
				return connection;
			}
            catch (Exception ex)
            {
                throw new Exception("Failed to open database connection", ex);
            }


        }
    }
}

