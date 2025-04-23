using GCGRA.UPDB.Core.Entities;

namespace GCGRA.UPDB.API.RequestModel
{
    public class UploadRequest
    {
        public List<Player> Players { get; set; } = new List<Player>();

    }
}
