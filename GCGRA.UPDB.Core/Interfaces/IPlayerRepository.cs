using GCGRA.UPDB.Core.Entities;

namespace GCGRA.UPDB.Core.Interfaces
{
    public interface IPlayerRepository
    {
        Task<IEnumerable<Player>> GetAllAsync();
        Task<Player> GetByIdAsync(int id);
    }
}
