using GCGRA.UPDB.Core.Entities;
using MediatR;

namespace GCGRA.UPDB.Application.Features.Blob.Commands
{
    public class UploadJsonToBlobCommand : IRequest<string>
    {
        public List<Player> players { get; set; }
        public string BlobName { get; set; }
        public UploadJsonToBlobCommand(List<Player> players, string blobName)
        {
            players = players;
            BlobName = blobName;
        }
    }
}
