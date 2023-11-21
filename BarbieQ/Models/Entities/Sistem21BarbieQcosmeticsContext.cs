using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BarbieQ.Models.Entities;

public partial class Sistem21BarbieQcosmeticsContext : DbContext
{
    public Sistem21BarbieQcosmeticsContext()
    {
    }

    public Sistem21BarbieQcosmeticsContext(DbContextOptions<Sistem21BarbieQcosmeticsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=sistemas19.com;database=sistem21_BarbieQCosmetics;user=sistem21_barbieqcosmetics;password=sistemas19_;convert zero datetime=True", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.5.20-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8_general_ci")
            .HasCharSet("utf8");

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("categoria");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Descripcion).HasColumnType("text");
            entity.Property(e => e.Nombre).HasMaxLength(200);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("cliente");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(200)
                .HasColumnName("Correo_electronico");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("producto");

            entity.HasIndex(e => e.IdCategoria, "fk_producto_categoria");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.CantidadExistencia)
                .HasDefaultValueSql("'0'")
                .HasColumnType("int(11)");
            entity.Property(e => e.Descripcion).HasColumnType("text");
            entity.Property(e => e.IdCategoria)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Categoria");
            entity.Property(e => e.Nombre).HasMaxLength(200);
            entity.Property(e => e.Precio).HasPrecision(10);

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_producto_categoria");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
