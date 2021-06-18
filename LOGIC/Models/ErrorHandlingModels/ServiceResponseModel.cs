using System.Collections.Generic;

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
        CategoryDoesntExist=5,
        InvalidToken=6,
        TokenExpired=7,
        TokenDoesntExist=8,
        RefreshTokenExpired=9,
        RefreshTokenInvalid=10,
        RefreshTokenUsed=11,
        RefreshTokenDoesntMatchJwT=12,
        UserDoesntExist=13,
        PassNickWrong=14,
    }
}
