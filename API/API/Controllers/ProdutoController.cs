using API.Data;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/produto")]
[Authorize]  
public class ProdutoController : ControllerBase
{
    private readonly IProdutoRepository _produtoRepository;
    public ProdutoController(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }
    
    [HttpGet("listar")]
    public IActionResult Listar()
    {
        return Ok(_produtoRepository.Listar());
    }

   
    [HttpGet("{id}")]
    public IActionResult Buscar([FromRoute] int id)
    {
        var produto = _produtoRepository.BuscarPorId(id);
        return produto != null ? Ok(produto) : NotFound();
    }

    [HttpPost("cadastrar")]
    [Authorize(Roles = "administrador")]
    public IActionResult Cadastrar([FromBody] Produto produto)
    {
        _produtoRepository.Cadastrar(produto);
        return Created("", produto);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "administrador")]
    public IActionResult Atualizar([FromRoute] int id, [FromBody] Produto produto)
    {
        var produtoExistente = _produtoRepository.BuscarPorId(id);
        if (produtoExistente == null)
        {
            return NotFound();
        }

        produtoExistente.Nome = produto.Nome;
        _produtoRepository.Atualizar(produtoExistente);
        
        return Ok(produtoExistente);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "administrador")]
    public IActionResult Deletar([FromRoute] int id)
    {
        var produto = _produtoRepository.BuscarPorId(id);
        if (produto == null)
        {
            return NotFound();
        }
        _produtoRepository.Deletar(id);
        return Ok(new { message = "Produto deletado com sucesso!" });
    }
}