using System.Net;

namespace FurniHub.Models.ApiResponseModel
{
    public class APIResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessages { get; set; }
        public T Result {  get; set; }
      
        public APIResponse(HttpStatusCode statusCode,bool isSuccess,string errorMessages,T result)
        {
            StatusCode = statusCode;
            IsSuccess = isSuccess;
            ErrorMessages = errorMessages;
            Result = result;
            
        }
    }
}
