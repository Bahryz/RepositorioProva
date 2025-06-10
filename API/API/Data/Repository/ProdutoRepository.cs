namespace API.Data;
using API.Models;

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDataContext _context;
    public ProdutoRepository(AppDataContext context)
    {
        _context = context;
    }

    public void Cadastrar(Produto produto)
    {
        _context.Produtos.Add(produto);
        _context.SaveChanges();
    }

    public List<Produto> Listar()
    {
        return _context.Produtos.ToList();
    }


    public Produto? BuscarPorId(int id)
    {
        return _context.Produtos.Find(id);
    }

    public void Atualizar(Produto produto)
    {
        _context.Produtos.Update(produto);
        _context.SaveChanges();
    }

    public void Deletar(int id)
    {
        var produto = _context.Produtos.Find(id);
        if (produto != null)
        {
            _context.Produtos.Remove(produto);
            _context.SaveChanges();
        }
    }
}