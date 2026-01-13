# DesafioIDez

## Visão Geral
API desenvolvida com o objetivo de fornecer informações sobre os municípios dos estados do Brasil,  
consumindo serviços externos oferecidos inicialmente pelos provedores: **BrasilApi** e **IBGE**.

## Tecnologias Utilizadas
Esta API foi desenvolvida utilizando **C#** e **.NET 10**.  
Também utilizamos **Redis** para trabalhar com cache distribuído.

## Arquitetura
O sistema implementa uma **arquitetura em camadas**, promovendo maior desacoplamento e boa organização  
de serviços externos e regras de negócio internas.  

Camadas principais:  
- **DesafioIDez.Api** → Camada de apresentação (API)  
- **DesafioIDez.Aplicacao** → Orquestração dos serviços formando os casos de uso da aplicação
- **DesafioIDez.Dominio** → Entidades e Interfaces para implementação dos Provedores 
- **DesafioIDez.Infraestrutura** → Redis e Provedores externos  

## Principais Endpoints

### GET `/api/ConsultaMunicipios/consulta`
Responsável por receber os parâmetros de consulta e retornar a listagem de municípios de acordo com o estado solicitado.

#### Exemplo de Requisição
```http
GET /api/ConsultaMunicipios/consulta?pagina=1&tamanhoPagina=10&estado=RS
```

#### Exemplo de Resposta

### 200 - OK

```http
{
  "pagina": 1,
  "tamanhoPagina": 20,
  "totalRegistros": 2,
  "totalPaginas" : 1,
  "itens": [
    {
      "name": "Aceguá",
      "ibgE_Code": "4300034"
    },
    {
      "name": "Água Santa",
      "ibgE_Code": "4300059"
    }
  ]
}
```

### 404/500 - Bad Request ou InternalServerError

```http
{
  "traceId": "3f29c8e2-7b4a-4d12-9f90-1a2b3c4d5e6f",
  "date": "13/01/2026 10:25:48",
  "source": "DesafioIDez.Api",
  "errorDetails": {
    "logReference": "LOG123456789",
    "innerException": "System.NullReferenceException: Object reference not set to an instance of an object.",
    "message": "Ocorreu um erro ao tentar processar a requisição.",
    "stackTrace": "at DesafioIDez.Controllers.ConsultaMunicipiosController.Consulta() in /src/DesafioIDez/Controllers/ConsultaMunicipiosController.cs:line 45"
  }
}
```

## Observação
Esta API utiliza **Redis** para gerenciamento de cache.  
Para garantir o funcionamento correto, é necessário ter uma instância do Redis em execução, seja local ou remota.

## Links Externos
- **API**: [https://desafioidez.simulados-backend.cloud](https://desafioidez.simulados-backend.cloud)  
- **Front-end**: [https://desafio-i-dez-front-end.vercel.app/](https://desafio-i-dez-front-end.vercel.app/)

