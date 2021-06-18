using DATA.Entities;
using DATA.Functions;
using DATA.Interfaces;
using LOGIC.Interfaces;
using LOGIC.Models;
using LOGIC.Models.ErrorHandlingModels;
using LOGIC.Models.TransactionModels;
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
        public IAccountFunctions _accountFunctions;
        public IArtFunctions _artFunctions;
        public ArtService(IAccountFunctions accountFunctions, IArtFunctions artFunctions)
        {
            _accountFunctions = accountFunctions;
            _artFunctions = artFunctions;
        }
        public async Task<ServiceResponseModel<ArtInfoModel>> AddNewImageAsync(string email, string name, string description, string category, IFormFile file)
        {

            var user = await _accountFunctions.GetUserByEmail(email);

            var userdata = await _accountFunctions.GetUserDataByUser(user);

            if (!(file.Length > 0))
            {
                return new ServiceResponseModel<ArtInfoModel>
                {
                    Success = false,
                    Errors = new List<Error>
                    {
                        new Error
                        {
                            Code=400,
                            Message=ErrorEnum.NoImageProvided
                        }
                    }
                };
            }

            string path = Directory.GetCurrentDirectory() + "\\uploads\\";

            DirectoryCreationCheck(path);

            string guid = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString();

            string extension = file.FileName[(file.FileName.LastIndexOf('.') + 1)..];

            SaveImageToDisk(path + guid + "." + extension, file);

            var dbcategory = _artFunctions.GetArtCategory(category);

            if (dbcategory == null)
            {
                return new ServiceResponseModel<ArtInfoModel>
                {
                    Success = false,
                    Errors = new List<Error>
                    {
                        new Error
                        {
                            Code=400,
                            Message=ErrorEnum.CategoryDoesntExist
                        }
                    }
                };
            }

            var art = new ArtData
            {
                Catgegory = dbcategory,
                CurrentOwner = userdata,
                Description = description,
                FileName = guid + "." + extension,
                Name = name,
                OriginalCreator = userdata
            };

            var DbArt = _artFunctions.AddArtData(userdata, art);

            return new ServiceResponseModel<ArtInfoModel>
            {
                Success = true,
                ResponseData= new ArtInfoModel
                {
                    Name=DbArt.Name,
                    Category=DbArt.Catgegory.CategoryName,
                    Description=DbArt.Description,
                    FileName= DbArt.FileName
                }
            };
        }

        public byte[] GetFileBytes(string fileName)
        {
            string path = Directory.GetCurrentDirectory() + "\\uploads\\";
            string filepath = path + fileName;
            if (File.Exists(filepath))
            {
                byte[] b = File.ReadAllBytes(filepath);

                string extension = fileName[(fileName.IndexOf('.') + 1)..];

                return b;
            }
            else return null;
        }

        public async Task<ServiceResponseModel> DeleteArt(string email, string fileName)
        {
            var user = await _accountFunctions.GetUserByEmail(email);

            var image = _artFunctions.GetArtDataByName(fileName);

            if (image == null) return new ServiceResponseModel
            {
                Success = false,
                Errors = new List<Error>
                {
                    new Error
                    {
                        Code=400,
                        Message= ErrorEnum.NotFound
                    }
                }
            };

            var permission = await _artFunctions.IsUserPermited(fileName, user, image);

            if (!permission) return new ServiceResponseModel
            {
                Success = false,
                Errors = new List<Error>
                {
                    new Error
                    {
                        Code=400,
                        Message= ErrorEnum.NotPermitted
                    }
                }
            };

            File.Delete(Directory.GetCurrentDirectory() + "\\uploads\\" + fileName);

            await _artFunctions.RemoveArtData(image);

            return new ServiceResponseModel
            {
                Success = false,
            };
        }

        public async Task<ServiceResponseModel<ArtListModel>> GetOwnedArt(string username)
        {
            var art = _artFunctions.GetOwnedArtByUserName(username);

            var owned = art.Select(i => new ArtDataModel
            {
                Id = i.Id,
                Name = i.Name,
                FileName = i.FileName,
                Description = i.Description,
                Catgegory = new ArtCategoryModel
                {
                    Id = i.Catgegory.Id,
                    Name = i.Catgegory.CategoryName
                }
            }).ToList();

            var ownedList = new ArtListModel
            {
                ArtList = owned
            };

            return new ServiceResponseModel<ArtListModel>
            {
                Success = true,
                ResponseData = ownedList
            };
        }

        public async Task<ServiceResponseModel<ArtListModel>> GetCreatedArt(string username)
        {
            var art = _artFunctions.GetCreatedArtByUserName(username);

            var owned = art.Select(i => new ArtDataModel
            {
                Id = i.Id,
                Name = i.Name,
                FileName = i.FileName,
                Description = i.Description,
                Catgegory = new ArtCategoryModel
                {
                    Id = i.Catgegory.Id,
                    Name = i.Catgegory.CategoryName
                }
            }).ToList();

            var ownedList = new ArtListModel
            {
                ArtList = owned
            };

            return new ServiceResponseModel<ArtListModel>
            {
                Success = true,
                ResponseData = ownedList
            };
        }

        private void DirectoryCreationCheck(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private void SaveImageToDisk(string path, IFormFile file)
        {
            using (FileStream fileStream = File.Create(path))
            {
                file.CopyTo(fileStream);

                fileStream.Flush();
            }
        }
    }
}
