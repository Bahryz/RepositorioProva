namespace API.Data;
using API.Models;

public interface IProdutoRepository
{
    void Cadastrar(Produto produto);
    List<Produto> Listar();
    Produto? BuscarPorId(int id); 
    void Atualizar(Produto produto); 
    void Deletar(int id);
}