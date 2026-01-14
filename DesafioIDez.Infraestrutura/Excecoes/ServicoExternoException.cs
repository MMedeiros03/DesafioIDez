namespace DesafioIDez.Infraestrutura.Excecoes
{
    public class ServicoExternoException : Exception
    {
        public ServicoExternoException(string message) : base(message) { }
    }
}
