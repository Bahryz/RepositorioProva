using Prova.Data;


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

    public Usuario? BuscarPorEmailSenha(string email, string senha)
    {
        return _context.Usuarios.FirstOrDefault(u => u.Email == email && u.Senha == senha);
    }

    public Usuario? BuscarPorEmail(string email)  
    {
        return _context.Usuarios.FirstOrDefault(x => x.Email == email);
    }
}