using LOGIC.Models;
using LOGIC.Models.TransactionModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC.Interfaces
{
    public interface IArtService
    {
        public Task<PostArtTransactionResult> AddNewImageAsync(string email, string name, string description, string category, IFormFile file);
        public byte[] GetFileBytes(string fileName);
        public Task<DeleteArtTransactionModel> DeleteArt(string email, string fileName);
        public ArtListModel GetCreatedArt(string username);
        public ArtListModel GetOwnedArt(string username);
    }
}
