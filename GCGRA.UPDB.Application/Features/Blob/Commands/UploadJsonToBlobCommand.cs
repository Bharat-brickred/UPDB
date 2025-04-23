using GCGRA.UPDB.Core.Entities;
using MediatR;

namespace GCGRA.UPDB.Application.Features.Blob.Commands
{
    public class UploadJsonToBlobCommand : IRequest<string>
    {
        public List<Player> Players { get; set; }
        public string BlobName { get; set; }
        public UploadJsonToBlobCommand(List<Player> Players, string blobName)
        {
            this.Players = Players;
            this.BlobName = blobName;
        }
    }
}
