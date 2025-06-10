using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;  
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
  


namespace Prova.Controllers;

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
    public IActionResult Cadastrar([FromBody] Usuario usuario)
    {
         
        _usuarioRepository.Cadastrar(usuario);
        return Created("", usuario);  
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] Usuario usuarioLogin)  
    {
        var usuarioExistente = _usuarioRepository.BuscarPorEmailSenha(usuarioLogin.Email, usuarioLogin.Senha);
        if (usuarioExistente == null)
        {
            return Unauthorized(new { mensagem = "Usuário ou senha inválidos!" });
        }

        var token = GerarToken(usuarioExistente);
        return Ok(new { token = token });  
    }

    [HttpGet("listar")]
    [Authorize]  
    public IActionResult Listar()
    {
        return Ok(_usuarioRepository.Listar());
    }

     
    [ApiExplorerSettings(IgnoreApi = true)]  
    public string GerarToken(Usuario usuario)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, usuario.Email),  
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) 
        };

        var chaveSecreta = _configuration["JwtSettings:SecretKey"];
        if (string.IsNullOrEmpty(chaveSecreta))
        {
            throw new InvalidOperationException("Chave secreta JWT não configurada no appsettings.json");
        }

        var chave = Encoding.UTF8.GetBytes(chaveSecreta);
        var assinatura = new SigningCredentials(new SymmetricSecurityKey(chave), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),  
            signingCredentials: assinatura
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}