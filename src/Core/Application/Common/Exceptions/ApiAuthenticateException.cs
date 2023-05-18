using System.Net;

namespace AACSB.WebApi.Application.Common.Exceptions;

public class ApiAuthenticateException : CustomException
{
    public ApiAuthenticateException(string message)
        : base(message, null, HttpStatusCode.Unauthorized)
    {
    }
}