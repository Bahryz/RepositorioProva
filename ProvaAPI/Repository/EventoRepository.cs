using Prova.Data;
using Prova.Repository.Interfaces;
using Microsoft.EntityFrameworkCore; 

namespace Prova.Repository;

public class EventoRepository : IEventoRepository
{
    private readonly AppDataContext _context;

    public EventoRepository(AppDataContext context)
    {
        _context = context;
    }

    public void Cadastrar(Evento evento)
    {
        _context.Eventos.Add(evento);
        _context.SaveChanges();
    }

    public List<Evento> Listar()
    {
         return _context.Eventos.Include(e => e.Usuario).ToList();
    }

    public List<Evento> ListarPorUsuario(int idUsuario)
    {
        return _context.Eventos
            .Include(e => e.Usuario) // Inclui dados do usuário
            .Where(e => e.UsuarioId == idUsuario) // Filtra por UsuarioId
            .ToList();
    }
}