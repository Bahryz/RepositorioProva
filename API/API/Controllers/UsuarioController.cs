using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Models;
using API.Data;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace API.Controllers;

[ApiController]
[Route("api/usuario")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _configuration;

    public UsuarioController(IUsuarioRepository usuarioRepository, IConfiguration configuration)
    {
        _usuarioRepository = usuarioRepository;
        _configuration = configuration;
    }

    [HttpPost("cadastrar")]
    [AllowAnonymous]
    public IActionResult Cadastrar([FromBody] Usuario usuario)
    {
        if (_usuarioRepository.BuscarPorEmail(usuario.Email) != null)
        {
            return BadRequest("Este e-mail já está em uso.");
        }
        _usuarioRepository.Cadastrar(usuario);
        return Created("", usuario);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] Usuario usuario)
    {
    Console.WriteLine($"Tentativa de login com Email: '{usuario.Email}' e Senha: '{usuario.Senha}'");

    Usuario? usuarioExistente = _usuarioRepository.BuscarUsuarioPorEmailSenha(usuario.Email, usuario.Senha);
    
    if (usuarioExistente == null)
    {
        Console.WriteLine("--> RESULTADO: Usuário não encontrado no banco com essas credenciais.");
        return Unauthorized(new { mensagem = "Usuário ou senha inválidos!" });
    }
    
    string token = GerarToken(usuarioExistente);

    return Ok(new
    {
        token = token,
        permissao = usuarioExistente.Permissao.ToString().ToLower()
    });
    }

    [HttpGet("listar")]
    [Authorize(Roles = "administrador")]
    public IActionResult Listar()
    {
        return Ok(_usuarioRepository.Listar());
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "administrador")]
    public IActionResult Buscar([FromRoute] int id)
    {
        var usuario = _usuarioRepository.BuscarPorId(id);
        return usuario != null ? Ok(usuario) : NotFound();
    }

    [HttpPut("{id}")]
    [Authorize]
    public IActionResult Atualizar([FromRoute] int id, [FromBody] Usuario usuario)
    {
        var usuarioLogadoEmail = User.Identity?.Name;
        var usuarioLogadoRole = User.FindFirst(ClaimTypes.Role)?.Value;

        var usuarioExistente = _usuarioRepository.BuscarPorId(id);
        if (usuarioExistente == null)
        {
            return NotFound();
        }

        if (usuarioExistente.Email != usuarioLogadoEmail && usuarioLogadoRole != "administrador")
        {
            return Forbid();
        }

        usuarioExistente.Email = usuario.Email;
        _usuarioRepository.Atualizar(usuarioExistente);
        return Ok(usuarioExistente);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "administrador")]
    public IActionResult Deletar([FromRoute] int id)
    {
        var usuario = _usuarioRepository.BuscarPorId(id);
        if (usuario == null)
        {
            return NotFound();
        }
        _usuarioRepository.Deletar(id);
        return Ok(new { message = "Usuário deletado com sucesso!" });
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public string GerarToken(Usuario usuario)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Permissao.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(8),
            signingCredentials: creds
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}