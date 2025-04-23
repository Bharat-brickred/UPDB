using System.Numerics;
using Azure;
using Azure.Data.Tables;
using GCGRA.UPDB.Core.Entities;
using GCGRA.UPDB.Core.Interfaces;

namespace GCGRA.UPDB.Infrastructure.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly DatabaseClient _databaseClient;

        public PlayerRepository(DatabaseClient databaseClient)
        {
            _databaseClient = databaseClient;
        }

        public async Task<IEnumerable<Player>> GetAllAsync()
        {
            return await _databaseClient.GetAllPlayersAsync();
        }

        public async Task<Player> GetByIdAsync(int id)
        {
            return await _databaseClient.GetPlayerByIdAsync(id);
        }
    }
}
