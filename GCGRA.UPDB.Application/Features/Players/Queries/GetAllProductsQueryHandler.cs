using GCGRA.UPDB.Core.Entities;
using GCGRA.UPDB.Core.Interfaces;
using MediatR;

namespace GCGRA.UPDB.Application.Features.Players.Queries
{
    internal class GetAllProductsQueryHandler : IRequestHandler<GetAllPlayersQuery, IEnumerable<Player>>
    {
        private readonly IPlayerRepository _playerRepository;

        public GetAllProductsQueryHandler(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public async Task<IEnumerable<Player>> Handle(GetAllPlayersQuery request, CancellationToken cancellationToken)
        {
            return await _playerRepository.GetAllAsync();
        }
    }
}
