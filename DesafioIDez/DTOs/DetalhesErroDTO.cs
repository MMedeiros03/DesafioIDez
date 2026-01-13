namespace DesafioIDez.Api.DTOs
{
    public class DetalhesErroDTO(string logReference, string innerException, string message, string stackTrace)
    {
        public string? LogReference { get; set; } = logReference;
        public string? InnerException { get; set; } = innerException;
        public string? Message { get; set; } = message;
        public string? StackTrace { get; set; } = stackTrace;
    }
}
