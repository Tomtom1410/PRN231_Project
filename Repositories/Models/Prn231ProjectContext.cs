using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Models;

public partial class Prn231ProjectContext : DbContext
{
    public Prn231ProjectContext()
    {
    }

    public Prn231ProjectContext(DbContextOptions<Prn231ProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseAccount> CourseAccounts { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=(local); database=PRN231_Project;uid=sa;pwd=NTQ@1234;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.Property(e => e.Dob).HasColumnType("date");
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasMany(d => d.Classes).WithMany(p => p.Accounts)
                .UsingEntity<Dictionary<string, object>>(
                    "ClassAccount",
                    r => r.HasOne<Class>().WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Class_Account_Classes"),
                    l => l.HasOne<Account>().WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Class_Account_Accounts"),
                    j =>
                    {
                        j.HasKey("AccountId", "ClassId");
                        j.ToTable("Class_Account");
                        j.IndexerProperty<long>("AccountId").HasColumnName("Account_id");
                        j.IndexerProperty<long>("ClassId").HasColumnName("Class_id");
                    });

            entity.HasMany(d => d.Documents).WithMany(p => p.Accounts)
                .UsingEntity<Dictionary<string, object>>(
                    "DocumentAccount",
                    r => r.HasOne<Document>().WithMany()
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Document_Account_Documents"),
                    l => l.HasOne<Account>().WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Document_Account_Accounts"),
                    j =>
                    {
                        j.HasKey("AccountId", "DocumentId");
                        j.ToTable("Document_Account");
                        j.IndexerProperty<long>("AccountId").HasColumnName("Account_id");
                        j.IndexerProperty<long>("DocumentId").HasColumnName("Document_id");
                    });
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.Property(e => e.ClassCode).HasMaxLength(50);
            entity.Property(e => e.ClassName).HasMaxLength(255);

            entity.HasMany(d => d.Courses).WithMany(p => p.Classes)
                .UsingEntity<Dictionary<string, object>>(
                    "ClassCourse",
                    r => r.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Class_Course_Courses"),
                    l => l.HasOne<Class>().WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Class_Course_Classes"),
                    j =>
                    {
                        j.HasKey("ClassId", "CourseId");
                        j.ToTable("Class_Course");
                        j.IndexerProperty<long>("ClassId").HasColumnName("Class_id");
                        j.IndexerProperty<long>("CourseId").HasColumnName("Course_id");
                    });
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.Property(e => e.CourseCode).HasMaxLength(50);
            entity.Property(e => e.CourseName).HasMaxLength(255);
        });

        modelBuilder.Entity<CourseAccount>(entity =>
        {
            entity.HasKey(e => new { e.AccountId, e.CourseId });

            entity.ToTable("Course_Account");

            entity.Property(e => e.AccountId).HasColumnName("Account_id");
            entity.Property(e => e.CourseId).HasColumnName("Course_id");

            entity.HasOne(d => d.Account).WithMany(p => p.CourseAccounts)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Course_Account_Accounts");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseAccounts)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Course_Account_Courses");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.Property(e => e.ContentType).HasMaxLength(50);
            entity.Property(e => e.DocumentName).HasMaxLength(50);
            entity.Property(e => e.DocumentOriginalName).HasMaxLength(50);
            entity.Property(e => e.PathFile).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
