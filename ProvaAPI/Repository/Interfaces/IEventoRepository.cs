namespace Prova.Repository.Interfaces;

public interface IEventoRepository
{
    void Cadastrar(Evento evento);
    List<Evento> Listar();
    List<Evento> ListarPorUsuario(int idUsuario);  
}