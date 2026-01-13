namespace DesafioIDez.Api.DTOs
{
    public class ErroDTO(string logReference, string innerException, string message, string stackTrace, string source)
    {
        public string? TraceId { get; set; } = Guid.NewGuid().ToString();
        public string? Date { get; set; } = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        public string? Source { get; set; } = source;
        public DetalhesErroDTO? ErrorDetails { get; set; } = new DetalhesErroDTO(logReference, innerException, message, stackTrace);
    }
}
