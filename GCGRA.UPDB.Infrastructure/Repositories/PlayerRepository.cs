using System.Numerics;
using Azure;
using Azure.Data.Tables;
using GCGRA.UPDB.Core.Entities;
using GCGRA.UPDB.Core.Interfaces;

namespace GCGRA.UPDB.Infrastructure.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly TableClient _tableClient;

        public PlayerRepository(string storageConnectionString, string tableName)
        {
            var serviceClient = new TableServiceClient(storageConnectionString);
            _tableClient = serviceClient.GetTableClient(tableName);
            _tableClient.CreateIfNotExists();
        }

        public async Task<IEnumerable<Player>> GetAllAsync()
        {
            var players = new List<Player>();

            await foreach (var entity in _tableClient.QueryAsync<TableEntity>())
            {
                var player = MapEntityToPlayer(entity);
                players.Add(player);
            }

            return players;
        }

        public async Task<Player> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _tableClient.GetEntityAsync<TableEntity>(id.ToString(), id.ToString());
                return MapEntityToPlayer(entity.Value);
            }
            catch (RequestFailedException)
            {
                return null;
            }
        }

        private Player MapEntityToPlayer(TableEntity entity)
        {
            var player = new Player();
            var playerType = typeof(Player);

            foreach (var property in playerType.GetProperties())
            {
                if (entity.ContainsKey(property.Name))
                {
                    var value = entity[property.Name];
                    if (value != null && property.CanWrite)
                    {
                        property.SetValue(player, Convert.ChangeType(value, property.PropertyType));
                    }
                }
            }

            return player;
        }
    }
}
