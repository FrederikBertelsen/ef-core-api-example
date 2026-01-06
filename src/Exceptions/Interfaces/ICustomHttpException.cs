namespace EfCoreApiTemplate.src.Exceptions.Interfaces;

public interface ICustomHttpException
{
    int StatusCode { get; }
}