using Art_Exchange_Token_System.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
