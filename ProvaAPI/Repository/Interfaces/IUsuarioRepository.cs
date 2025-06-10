public interface IUsuarioRepository
{
    void Cadastrar(Usuario usuario);
    List<Usuario> Listar();
    Usuario? BuscarPorEmailSenha(string email, string senha);  
    Usuario? BuscarPorEmail(string email);  
}