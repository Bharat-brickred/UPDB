using GCGRA.UPDB.Core.Entities;
using MediatR;

namespace GCGRA.UPDB.Application.Features.Players.Queries
{
    public class GetAllPlayersQuery : IRequest<IEnumerable<Player>>
    {
    }
}
