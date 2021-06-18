using Microsoft.AspNetCore.Http;

namespace Art_Exchange_Token_System.Models.RequestModels
{
    public class PostArtModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public IFormFile File { get; set; }
    }
}
