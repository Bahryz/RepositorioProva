namespace API.Data;  
using API.Models;

public interface IUsuarioRepository
{
    void Cadastrar(Usuario usuario);
    List<Usuario> Listar();
    Usuario? BuscarPorId(int id);
    Usuario? BuscarPorEmail(string email);  
    Usuario? BuscarUsuarioPorEmailSenha(string email, string senha);
    void Atualizar(Usuario usuario);
    void Deletar(int id);
}
