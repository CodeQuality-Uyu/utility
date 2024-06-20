namespace CQ.Utility;
public class RequestException<TError> : Exception where TError : class
{
    public readonly TError ErrorBody;

    public RequestException(TError errorBody)
    {
        this.ErrorBody = errorBody;
    }
}
