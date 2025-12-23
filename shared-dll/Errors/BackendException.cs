namespace Shrak.Errors;

public class BackendException : Exception
{
    public Error Error { get; set; }

    public static BackendException Create(Error error)
    {
        return new BackendException()
        {
            Error = error
        };
    }

    public static BackendException Create<T>() where T : Error, new()
    {
        return new BackendException()
        {
            Error = new T()
            {
                Message = typeof(T).Name
            }
        };
    }

    public override string Message => Error != null ? $"{Error.GetType()}|{Error}" : base.Message;
}

