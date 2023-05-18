namespace AACSB.WebApi.Application.Common.Exceptions;

public class RedirectToLoginException : CustomException
{
    public RedirectToLoginException(string message) : base(message) { }

    public RedirectToLoginException(string message, params object[] args)
        : base(string.Format(message, args))
    {
    }
}