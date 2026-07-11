using Library.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Infrastructure.Data;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Borrowing> Borrowings => Set<Borrowing>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(b =>
        {
            b.ToTable("Books");
            b.HasKey(x => x.Id);
            b.Property(x => x.Title).IsRequired().HasMaxLength(300);
            b.Property(x => x.Author).IsRequired().HasMaxLength(200);
            b.Property(x => x.Isbn).IsRequired().HasMaxLength(20);
            b.HasIndex(x => x.Isbn).IsUnique();
        });

        modelBuilder.Entity<Member>(m =>
        {
            m.ToTable("Members");
            m.HasKey(x => x.Id);
            m.Property(x => x.FullName).IsRequired().HasMaxLength(200);
            m.Property(x => x.Email).IsRequired().HasMaxLength(200);
            m.HasIndex(x => x.Email).IsUnique();
        });

        modelBuilder.Entity<Borrowing>(br =>
        {
            br.ToTable("Borrowings");
            br.HasKey(x => x.Id);
            br.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);

            br.HasOne(x => x.Book)
              .WithMany()
              .HasForeignKey(x => x.BookId)
              .OnDelete(DeleteBehavior.Restrict);

            br.HasOne(x => x.Member)
              .WithMany()
              .HasForeignKey(x => x.MemberId)
              .OnDelete(DeleteBehavior.Restrict);
        });

        base.OnModelCreating(modelBuilder);
    }
}
