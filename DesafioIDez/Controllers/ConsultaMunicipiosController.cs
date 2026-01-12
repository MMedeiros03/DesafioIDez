using DesafioIDez.Aplicacao.Interfaces.Factory;
using DesafioIDez.Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace DesafioIDez.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultaMunicipiosController(IConsultaMunicipiosFactory consultaMunicipiosFactory, IConfiguration configuration) : ControllerBase
    {
        private readonly IConsultaMunicipiosFactory _consultaMunicipiosFactory = consultaMunicipiosFactory;
        private readonly IConfiguration _configuration = configuration;

        [HttpGet]
        [Route("consulta")]
        public async Task<ActionResult<List<Municipio>>> ConsultarMunicipiosPorEstado([FromQuery] string estado)
        {
            var providerConfig = _configuration["Provider"]
                ?? throw new ArgumentException("Não foi possivel obter o provedor selecionado.");

            var servico = _consultaMunicipiosFactory.ObterConsulta(providerConfig);

            return await servico.ConsultarMunicipiosPorEstadoAsync(estado);
        }
    }
}
