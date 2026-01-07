using EfCoreApiExample.src.Exceptions.Interfaces;

namespace EfCoreApiExample.src.Exceptions;

public class BusinessLogicException(string message) : Exception(message.ToLower()), ICustomHttpException
{
    public int StatusCode => 400;
}