
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using GCGRA.UPDB.Core.Entities;

namespace GCGRA.UPDB.Infrastructure
{
    public class DatabaseClient
    {
        private readonly string _connectionString;

        public DatabaseClient(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Player>> GetAllPlayersAsync()
        {
            using (var connection = CreateConnection())
            {
                var sql = "SELECT * FROM Player"; try
                {
                    var result = await connection.QueryAsync<Player>(sql);
                  //  _logger.LogInformation("Query executed successfully. Number of records: {Count}", result?.Count() ?? 0);
                    return result;
                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex, "An error occurred while executing the SQL query.");
                    throw;
                }
            }
        }

        public async Task<Player> GetPlayerByIdAsync(int id)
        {
            using (var connection = CreateConnection())
            {
                var sql = "SELECT * FROM Player WHERE PAI_0000200_Operator_Player_ID = @Id";
                try
                {
                    var result = await connection.QuerySingleOrDefaultAsync<Player>(sql, new { Id = id });
                    if (result == null)
                    {
                       // _logger.LogWarning("No player found with ID: {Id}", id);
                    }
                    else
                    {
                       // _logger.LogInformation("Query executed successfully. Player found: {Player}", result);
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex, "An error occurred while executing the SQL query.");
                    throw;
                }
            }
        }
    }
}
