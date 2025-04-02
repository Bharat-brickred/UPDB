using MediatR;
using GCGRA.UPDB.Core.Entities;

namespace GCGRA.UPDB.Application.Features.Players.Queries
{
    public class GetPlayerByIdQuery : IRequest<Player>
    {
        public int Id { get; set; }
        public GetPlayerByIdQuery(int id) => Id = id;
    }
}
