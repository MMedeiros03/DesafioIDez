namespace DesafioIDez.Aplicacao.DTO;

public class ListaPaginadaRDTO<T>
{
    public int Pagina { get; set; }
    public int TamanhoPagina { get; set; }
    public int TotalItens { get; set; }
    public int TotalPaginas { get; set; }
    public List<T> Itens { get; set; } = [];
}
