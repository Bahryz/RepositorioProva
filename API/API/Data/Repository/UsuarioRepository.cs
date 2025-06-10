using API.Models;

namespace API.Data;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDataContext _context;
    public UsuarioRepository(AppDataContext context)
    {
        _context = context;
    }

    public void Cadastrar(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        _context.SaveChanges();
    }

    public List<Usuario> Listar()
    {
        return _context.Usuarios.ToList();
    }

    public Usuario? BuscarUsuarioPorEmailSenha(string email, string senha)
    {
        return _context.Usuarios.FirstOrDefault(x => x.Email == email && x.Senha == senha);
    }

    public Usuario? BuscarPorId(int id)
    {
        return _context.Usuarios.Find(id);
    }

    public Usuario? BuscarPorEmail(string email)
    {
        return _context.Usuarios.FirstOrDefault(x => x.Email == email);
    }

    public void Atualizar(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        _context.SaveChanges();
    }

    public void Deletar(int id)
    {
        var usuario = _context.Usuarios.Find(id);
        if (usuario != null)
        {
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
        }
    }
}