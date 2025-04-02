using GCGRA.UPDB.Core.Entities;
using GCGRA.UPDB.Core.Interfaces;

namespace GCGRA.UPDB.Infrastructure.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly List<Player> _players = new()
        {
            new Player { Id = 1, Name = "Laptop", Price = 1200 },
            new Player { Id = 2, Name = "Smartphone", Price = 800 }
        };

        public async Task<IEnumerable<Player>> GetAllAsync()
        {
            return await Task.FromResult(_players);
        }

        public async Task<Player> GetByIdAsync(int id)
        {
            var Player = _players.FirstOrDefault(p => p.Id == id);
            return await Task.FromResult(Player);
        }
    }
}
