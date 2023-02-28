using App.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Context;

public class DatabaseContext : DbContext
{
    public DbSet<ImagemModel> Imagens { get; set; }
    public DbSet<GaleriaModel> Galerias { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ImagemModel>().ToTable("Imagem");
        modelBuilder.Entity<GaleriaModel>().ToTable("Galeria");
    }



}