using LOGIC.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LOGIC.Services
{
    public class ArtService: IArtService
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
            using (FileStream fileStream = File.Create(path))
            {
                file.CopyTo(fileStream);

                fileStream.Flush();
            }
        }
    }
}
