using Microsoft.Data.SqlClient;

namespace JobApplicationTracker.Api.Services.Interfaces
{
    public interface IDatabaseConnectionService
    {
        Task<SqlConnection> GetDatabaseConnectionAsync();
        Task CloseDatabaseConnectionAsync(SqlConnection connection);
    }
}
