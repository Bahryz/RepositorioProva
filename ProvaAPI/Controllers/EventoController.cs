using Prova.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;  

 
namespace Prova.Controllers;

[ApiController]
[Route("api/evento")]
public class EventoController : ControllerBase
{
    private readonly IEventoRepository _eventoRepository;
    private readonly IUsuarioRepository _usuarioRepository;  

    public EventoController(IEventoRepository eventoRepository, IUsuarioRepository usuarioRepository)
    {
        _eventoRepository = eventoRepository;
        _usuarioRepository = usuarioRepository;
    }

    [HttpPost("cadastrar")]
    [Authorize]  
    public IActionResult Cadastrar([FromBody] Evento evento)
    {
         var emailUsuarioLogado = User.FindFirstValue(ClaimTypes.Name);  
        if (string.IsNullOrEmpty(emailUsuarioLogado))
        {
            return Unauthorized("Não foi possível identificar o usuário logado.");
        }

        var usuarioLogado = _usuarioRepository.BuscarPorEmail(emailUsuarioLogado);
        if (usuarioLogado == null)
        {
            return NotFound("Usuário associado ao token não encontrado.");
        }

        evento.UsuarioId = usuarioLogado.Id;
 
        _eventoRepository.Cadastrar(evento);
          return Created("", evento);  
    }

    [HttpGet("listar")]
     public IActionResult Listar()
    {
      
        return Created("", _eventoRepository.Listar());  
    }
    
    [HttpGet("usuario")]  
    [Authorize]  
    public IActionResult ListarEventosDoUsuario()
    {
        var emailUsuarioLogado = User.FindFirstValue(ClaimTypes.Name);
        if (string.IsNullOrEmpty(emailUsuarioLogado))
        {
            
            return Unauthorized("Não foi possível identificar o usuário logado.");
        }

        var usuarioLogado = _usuarioRepository.BuscarPorEmail(emailUsuarioLogado);
        if (usuarioLogado == null)
        {
            return NotFound("Usuário não encontrado.");
        }
       
        return Created("", _eventoRepository.ListarPorUsuario(usuarioLogado.Id));  
    }
}