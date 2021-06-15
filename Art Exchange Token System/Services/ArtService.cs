using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Art_Exchange_Token_System.Services
{
    public class ArtService
    {
        public void DirectoryCreationCheck(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public void SaveImageToDisk(string path, IFormFile file)
        {
            using (FileStream fileStream = System.IO.File.Create(path))
            {
                file.CopyTo(fileStream);

                fileStream.Flush();
            }
        }
    }
}
