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

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<Cliente> Cliente { get; set; }

    public virtual DbSet<Producto> Producto { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8_general_ci")
            .HasCharSet("utf8");

        modelBuilder.Entity<Categoria>(entity =>
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
            entity.Property(e => e.Contrasena)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(200)
                .HasColumnName("Correo_electronico");
            entity.Property(e => e.Nombre).HasMaxLength(200);
            entity.Property(e => e.Rol).HasColumnType("int(11)");
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
            entity.Property(e => e.Ingredientes).HasColumnType("text");
            entity.Property(e => e.Nombre).HasMaxLength(200);
            entity.Property(e => e.Precio).HasPrecision(10);

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Producto)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_producto_categoria");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
