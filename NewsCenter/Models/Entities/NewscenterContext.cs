using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace NewsCenter.Models.Entities;

public partial class NewscenterContext : DbContext
{
    public NewscenterContext()
    {
    }

    public NewscenterContext(DbContextOptions<NewscenterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Autores> Autores { get; set; }

    public virtual DbSet<Ciudades> Ciudades { get; set; }

    public virtual DbSet<Noticias> Noticias { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=root;database=newscenter", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.36-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Autores>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("autores");

            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Ciudades>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ciudades");

            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Noticias>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("noticias");

            entity.HasIndex(e => e.AutorId, "AutorId");

            entity.HasIndex(e => e.CiudadId, "CiudadId");

            entity.Property(e => e.Contenido).HasColumnType("text");
            entity.Property(e => e.Titulo).HasMaxLength(255);

            entity.HasOne(d => d.Autor).WithMany(p => p.Noticias)
                .HasForeignKey(d => d.AutorId)
                .HasConstraintName("noticias_ibfk_2");

            entity.HasOne(d => d.Ciudad).WithMany(p => p.Noticias)
                .HasForeignKey(d => d.CiudadId)
                .HasConstraintName("noticias_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
