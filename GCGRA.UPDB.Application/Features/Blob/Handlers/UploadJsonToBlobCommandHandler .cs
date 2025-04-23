using GCGRA.UPDB.Application.Features.Blob.Commands;
using GCGRA.UPDB.Core.Interfaces;
using MediatR;

namespace GCGRA.UPDB.Application.Features.Blob.Handlers
{
    public class UploadJsonToBlobCommandHandler : IRequestHandler<UploadJsonToBlobCommand, string>
    {
        private readonly IBlobStorageService _blobStorageService;

        public UploadJsonToBlobCommandHandler(IBlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        public async Task<string> Handle(UploadJsonToBlobCommand request, CancellationToken cancellationToken)
        {
            // Use the BlobStorageService to upload the JSON data to Blob Storage
            return await _blobStorageService.UploadJsonToBlobAsync(request.Players, request.BlobName);
        }
    }
}
