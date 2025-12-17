using Shrak.Errors;

namespace Shrak.Protocols;

public class Response
{
    public Error? Error { get; set; }
    public DateTime TimeStamp { get; set; }
}

