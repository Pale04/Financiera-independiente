using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Data_Access.Entities;

public partial class independent_financialContext : DbContext
{
    private string _connectionString;

    public independent_financialContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public independent_financialContext(DbContextOptions<independent_financialContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BankAccount> BankAccounts { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Credit> Credits { get; set; }

    public virtual DbSet<CreditCondition> CreditConditions { get; set; }

    public virtual DbSet<CreditPolicy> CreditPolicies { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PersonalReference> PersonalReferences { get; set; }

    public virtual DbSet<RequiredDocumentation> RequiredDocumentations { get; set; }

    public virtual DbSet<Subsidiary> Subsidiaries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(_connectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__BankAcco__3213E83FA2007890");

            entity.ToTable("BankAccount");

            entity.Property(e => e.clabe)
                .HasMaxLength(18)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.clientId)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.purpose)
                .HasMaxLength(7)
                .IsUnicode(false);

            entity.HasOne(d => d.client).WithMany(p => p.BankAccounts)
                .HasForeignKey(d => d.clientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BankAccou__clien__6166761E");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.rfc).HasName("PK__Client__C2B034958D2CF976");

            entity.ToTable("Client");

            entity.Property(e => e.rfc)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.houseAddress)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.mail)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.phoneNumber1)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.phoneNumber2)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.workAddress)
                .HasMaxLength(400)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Credit>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Credit__3213E83F987AD812");

            entity.ToTable("Credit");

            entity.Property(e => e.beneficiary)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.registryDate).HasColumnType("datetime");
            entity.Property(e => e.state)
                .HasMaxLength(14)
                .IsUnicode(false);

            entity.HasOne(d => d.beneficiaryNavigation).WithMany(p => p.Credits)
                .HasForeignKey(d => d.beneficiary)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Credit__benefici__5BAD9CC8");

            entity.HasOne(d => d.condition).WithMany(p => p.Credits)
                .HasForeignKey(d => d.conditionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Credit__conditio__5D95E53A");

            entity.HasOne(d => d.registrerNavigation).WithMany(p => p.Credits)
                .HasForeignKey(d => d.registrer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Credit__registre__5CA1C101");
        });

        modelBuilder.Entity<CreditCondition>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__CreditCo__3213E83F9E68C71F");

            entity.ToTable("CreditCondition");

            entity.HasOne(d => d.registrerNavigation).WithMany(p => p.CreditConditions)
                .HasForeignKey(d => d.registrer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CreditCon__regis__6442E2C9");
        });

        modelBuilder.Entity<CreditPolicy>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__CreditPo__3213E83F0018D63C");

            entity.ToTable("CreditPolicy");

            entity.Property(e => e.description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.title)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.registrerNavigation).WithMany(p => p.CreditPolicies)
                .HasForeignKey(d => d.registrer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CreditPol__regis__65370702");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Document__3213E83F5CB210EA");

            entity.ToTable("Document");

            entity.Property(e => e.file).HasMaxLength(8000);
            entity.Property(e => e.name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.registryDate).HasColumnType("datetime");

            entity.HasOne(d => d.credit).WithMany(p => p.Documents)
                .HasForeignKey(d => d.creditId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Document__credit__607251E5");

            entity.HasOne(d => d.documentation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.documentationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Document__docume__5F7E2DAC");

            entity.HasOne(d => d.registrerNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.registrer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Document__regist__5E8A0973");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Employee__3213E83F938BD96C");

            entity.ToTable("Employee");

            entity.Property(e => e.address)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.mail)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.password)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.phoneNumber)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.role)
                .HasMaxLength(9)
                .IsUnicode(false);
            entity.Property(e => e.user)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.sucursal).WithMany(p => p.Employees)
                .HasForeignKey(d => d.sucursalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employee__sucurs__5AB9788F");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Payment__3213E83F9A3D18A9");

            entity.ToTable("Payment");

            entity.Property(e => e.amount).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.state)
                .HasMaxLength(13)
                .IsUnicode(false);

            entity.HasOne(d => d.credit).WithMany(p => p.Payments)
                .HasForeignKey(d => d.creditId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__creditI__634EBE90");

            entity.HasOne(d => d.registrerNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.registrer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__registr__625A9A57");
        });

        modelBuilder.Entity<PersonalReference>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Personal__3213E83FE8ED8FB1");

            entity.ToTable("PersonalReference");

            entity.Property(e => e.clientRfc)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.phoneNumber)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.relationship)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.clientRfcNavigation).WithMany(p => p.PersonalReferences)
                .HasForeignKey(d => d.clientRfc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PersonalR__clien__662B2B3B");
        });

        modelBuilder.Entity<RequiredDocumentation>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Required__3213E83F1D20E2E3");

            entity.ToTable("RequiredDocumentation");

            entity.Property(e => e.fileType)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Subsidiary>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Subsidia__3213E83FBB5BF497");

            entity.ToTable("Subsidiary");

            entity.Property(e => e.address)
                .HasMaxLength(400)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
