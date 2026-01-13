namespace DesafioIDez.Aplicacao.DTO;

public class FiltroEstadoDto
{
    public int Pagina { get; set; }
    public int TamanhoPagina { get; set; }
    public required string Estado { get; set; }
    public string? Municipio { get; set; } = string.Empty;
    public string? Codigo_Ibge { get; set; } = string.Empty;
}
