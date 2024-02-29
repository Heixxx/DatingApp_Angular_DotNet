
namespace API.Errors;

public class ApiException{   // prop

    public int StatusCode {get;set;}
    public string Message { get; set; }
    public int MyProperty { get; set; }
    public string Details { get; set; }

    public ApiException(int statusCode, string message, string details) {
        StatusCode = statusCode;
        Message = message;
        Details = details;
    }

}