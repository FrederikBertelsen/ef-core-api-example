using EfCoreApiTemplate.src.Exceptions.Interfaces;

namespace EfCoreApiTemplate.src.Exceptions;

public class BusinessLogicException(string message) : Exception(message.ToLower()), ICustomHttpException
{
    public int StatusCode => 400;
}