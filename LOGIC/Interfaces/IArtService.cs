using LOGIC.Models;
using LOGIC.Models.ErrorHandlingModels;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LOGIC.Interfaces
{
    public interface IArtService
    {
        public Task<ServiceResponseModel<ArtInfoModel>> AddNewImageAsync(string email, string name, string description, string category, IFormFile file);
        public byte[] GetFileBytes(string fileName);
        public Task<ServiceResponseModel> DeleteArt(string email, string fileName);
        public Task<ServiceResponseModel<ArtListModel>> GetOwnedArt(string username);
        public Task<ServiceResponseModel<ArtListModel>> GetCreatedArt(string username);
    }
}
