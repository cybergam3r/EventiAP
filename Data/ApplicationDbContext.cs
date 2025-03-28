using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Artista> Artisti { get; set; }
    public DbSet<Evento> Eventi { get; set; }
    public DbSet<Biglietto> Biglietti { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Biglietto>().HasIndex(b => b.UserId);

        modelBuilder.Entity<Biglietto>()
            .HasOne(b => b.Evento)
            .WithMany(e => e.Biglietti)
            .HasForeignKey(b => b.EventoId);

        modelBuilder.Entity<Biglietto>()
            .HasOne(b => b.User)
            .WithMany()
            .HasForeignKey(b => b.UserId);
    }
}
