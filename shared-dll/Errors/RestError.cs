namespace Shrak.Errors;

public class RestError : Error
{

    public RestError()
    {
    }
    
    public RestError(string message)
    {
        Message = message;
    }
}