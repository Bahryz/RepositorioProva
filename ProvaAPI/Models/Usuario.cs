public class Usuario
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty; // Idealmente, armazene um hash da senha
    public DateTime CriadoEm { get; set; } = DateTime.Now;

    // Uma boa prática
    // public List<Evento> Eventos { get; set; } = new();
}