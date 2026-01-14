using System.Net;

namespace DesafioIDez.Infraestrutura.Excecoes;

public abstract class AppException : Exception
{
    protected AppException(string message) : base(message) { }
    public abstract HttpStatusCode StatusCode { get; }
}
