using Microsoft.EntityFrameworkCore;

namespace CadastroService.Models.Context
{
    public class LeitorDbContext : DbContext
    {
        public LeitorDbContext(DbContextOptions<LeitorDbContext> options): base(options)
        {
            
        }

        public DbSet<Leitor> Leitores { get; set; }
    }
}
