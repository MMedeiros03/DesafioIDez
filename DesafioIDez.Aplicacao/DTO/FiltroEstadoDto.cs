namespace DesafioIDez.Aplicacao.DTO;

public class FiltroEstadoDto
{
    public int Pagina { get; set; }
    public int TamanhoPagina { get; set; }
    public required string Estado { get; set; }
}
