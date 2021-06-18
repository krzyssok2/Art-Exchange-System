using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC.Models.ErrorHandlingModels
{
    public class ServiceResponseModel<T>
    {
        public bool Success { get; set; }
        public List<Error> Errors { get; set; }
        public T ResponseData { get; set; }
    }

    public class ServiceResponseModel
    {
        public bool Success { get; set; }
        public List<Error> Errors { get; set; }
    }

    public class Error
    {
        public short Code { get; set; }
        public ErrorEnum Message { get; set; }
    }

    public enum ErrorEnum
    {
        NotFound=0,
        NotPermitted=1,
        FailedToCreate=2,
        ArtNotOwned=3,
        NoImageProvided=4,
        CategoryDoesntExist=5
    }
}
