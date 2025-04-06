using GCGRA.UPDB.Core.Entities;

namespace GCGRA.UPDB.Core.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> UploadJsonToBlobAsync(object data, string blobName);
    }   
}
