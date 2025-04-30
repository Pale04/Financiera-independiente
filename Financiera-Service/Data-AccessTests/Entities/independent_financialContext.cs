using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Data_AccessTests.Entities;

public partial class independent_financialContext : DbContext
{
    private readonly string _connectionString;

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

    public virtual DbSet<RequiredDocumentation> RequiredDocumentations { get; set; }

    public virtual DbSet<Subsidiary> Subsidiaries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(_connectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__BankAcco__3213E83FF226B6BA");

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
                .HasConstraintName("FK__BankAccou__clien__5441852A");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.rfc).HasName("PK__Client__C2B0349579A9D9E7");

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
            entity.HasKey(e => e.id).HasName("PK__Credit__3213E83F28CCC092");

            entity.ToTable("Credit");

            entity.Property(e => e.beneficiary)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.state)
                .HasMaxLength(14)
                .IsUnicode(false);

            entity.HasOne(d => d.beneficiaryNavigation).WithMany(p => p.Credits)
                .HasForeignKey(d => d.beneficiary)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Credit__benefici__4E88ABD4");

            entity.HasOne(d => d.condition).WithMany(p => p.Credits)
                .HasForeignKey(d => d.conditionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Credit__conditio__5070F446");

            entity.HasOne(d => d.registrerNavigation).WithMany(p => p.Credits)
                .HasForeignKey(d => d.registrer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Credit__registre__4F7CD00D");
        });

        modelBuilder.Entity<CreditCondition>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__CreditCo__3213E83F2B32D4ED");

            entity.ToTable("CreditCondition");

            entity.HasOne(d => d.registrerNavigation).WithMany(p => p.CreditConditions)
                .HasForeignKey(d => d.registrer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CreditCon__regis__571DF1D5");
        });

        modelBuilder.Entity<CreditPolicy>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__CreditPo__3213E83FC74E905F");

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
                .HasConstraintName("FK__CreditPol__regis__5812160E");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Document__3213E83F789E6280");

            entity.ToTable("Document");

            entity.Property(e => e.name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.registryDate).HasColumnType("datetime");

            entity.HasOne(d => d.credit).WithMany(p => p.Documents)
                .HasForeignKey(d => d.creditId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Document__credit__534D60F1");

            entity.HasOne(d => d.documentation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.documentationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Document__docume__52593CB8");

            entity.HasOne(d => d.registrerNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.registrer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Document__regist__5165187F");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Employee__3213E83FF33F4612");

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
                .HasConstraintName("FK__Employee__sucurs__4D94879B");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Payment__3213E83FEB91B59A");

            entity.ToTable("Payment");

            entity.Property(e => e.amount).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.state)
                .HasMaxLength(13)
                .IsUnicode(false);

            entity.HasOne(d => d.credit).WithMany(p => p.Payments)
                .HasForeignKey(d => d.creditId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__creditI__5629CD9C");

            entity.HasOne(d => d.registrerNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.registrer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__registr__5535A963");
        });

        modelBuilder.Entity<RequiredDocumentation>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Required__3213E83F3655C7D8");

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
            entity.HasKey(e => e.id).HasName("PK__Subsidia__3213E83FC658DE36");

            entity.ToTable("Subsidiary");

            entity.Property(e => e.Address)
                .HasMaxLength(400)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
