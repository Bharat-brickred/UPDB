using GCGRA.UPDB.Core.Entities;
using GCGRA.UPDB.Core.Interfaces;
using MediatR;

namespace GCGRA.UPDB.Application.Features.Players.Queries
{
    internal class GetPlayerByIdQueryHandler:IRequestHandler<GetPlayerByIdQuery, Player>
    {
        private readonly IPlayerRepository _playerRepository;

        public GetPlayerByIdQueryHandler(IPlayerRepository PlayerRepository)
        {
            _playerRepository = PlayerRepository;
        }

        public async Task<Player> Handle(GetPlayerByIdQuery request, CancellationToken cancellationToken)
        {
            return await _playerRepository.GetByIdAsync(request.Id);
        }
    }
}
