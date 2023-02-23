using System.Net;

namespace AACSB.WebApi.Application.Common.Exceptions;

public class ForeignResourceErrorException : CustomException
{
    public ForeignResourceErrorException(string message, List<string>? errors = default)
        : base(message, errors, HttpStatusCode.InternalServerError)
    {
    }
}