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

    public virtual DbSet<CreditRequest> CreditRequests { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentLayout> PaymentLayouts { get; set; }

    public virtual DbSet<PersonalReference> PersonalReferences { get; set; }

    public virtual DbSet<RequiredDocumentation> RequiredDocumentations { get; set; }

    public virtual DbSet<Subsidiary> Subsidiaries { get; set; }

    public virtual DbSet<CreditPayment> CreditPayments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(_connectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__BankAcco__3213E83FAF3E8E2D");

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
                .HasConstraintName("FK__BankAccou__clien__2704CA5F");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.rfc).HasName("PK__Client__C2B03495C0D01D7F");

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
            entity.HasKey(e => e.id).HasName("PK__Credit__3213E83FE1C4285C");

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
                .HasConstraintName("FK__Credit__benefici__214BF109");

            entity.HasOne(d => d.condition).WithMany(p => p.Credits)
                .HasForeignKey(d => d.conditionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Credit__conditio__2334397B");

            entity.HasOne(d => d.registrerNavigation).WithMany(p => p.Credits)
                .HasForeignKey(d => d.registrer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Credit__registre__22401542");
        });

        modelBuilder.Entity<CreditCondition>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__CreditCo__3213E83F3918E3C6");

            entity.ToTable("CreditCondition");

            entity.HasOne(d => d.registrerNavigation).WithMany(p => p.CreditConditions)
                .HasForeignKey(d => d.registrer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CreditCon__regis__29E1370A");
        });

        modelBuilder.Entity<CreditPolicy>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__CreditPo__3213E83FFC172135");

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
                .HasConstraintName("FK__CreditPol__regis__2AD55B43");
        });

        modelBuilder.Entity<CreditRequest>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("CreditRequest");

            entity.Property(e => e.name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CreditPayment>(entity =>
        {
            entity
               .HasNoKey()
               .ToView("CreditPayment");
            entity.Property(e => e.conditionId).IsRequired();
            entity.Property(e => e.interestRate).IsRequired();
            entity.Property(e => e.paymentsPerMonth).IsRequired();
        });

        OnModelCreatingPartial(modelBuilder);

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Document__3213E83FBBFC86DA");

            entity.ToTable("Document");

            entity.Property(e => e.name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.registryDate).HasColumnType("datetime");

            entity.HasOne(d => d.credit).WithMany(p => p.Documents)
                .HasForeignKey(d => d.creditId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Document__credit__2610A626");

            entity.HasOne(d => d.documentation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.documentationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Document__docume__251C81ED");

            entity.HasOne(d => d.registrerNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.registrer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Document__regist__24285DB4");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Employee__3213E83F6D6580EA");

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
                .HasConstraintName("FK__Employee__sucurs__2057CCD0");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Payment__3213E83F63AA7A32");

            entity.ToTable("Payment");

            entity.Property(e => e.amount).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.state)
                .HasMaxLength(13)
                .IsUnicode(false);

            entity.HasOne(d => d.credit).WithMany(p => p.Payments)
                .HasForeignKey(d => d.creditId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__creditI__28ED12D1");

            entity.HasOne(d => d.registrerNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.registrer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__registr__27F8EE98");
        });

        modelBuilder.Entity<PaymentLayout>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("PaymentLayout");

            entity.Property(e => e.amount).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.clabe)
                .HasMaxLength(18)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PersonalReference>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Personal__3213E83F7D1D131C");

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
                .HasConstraintName("FK__PersonalR__clien__2BC97F7C");
        });

        modelBuilder.Entity<RequiredDocumentation>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Required__3213E83F76447188");

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
            entity.HasKey(e => e.id).HasName("PK__Subsidia__3213E83FEAEC09F4");

            entity.ToTable("Subsidiary");

            entity.Property(e => e.address)
                .HasMaxLength(400)
                .IsUnicode(false);
        });

        
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
