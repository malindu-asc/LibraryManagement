using Microsoft.EntityFrameworkCore;

namespace Library.Api.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(LibraryDbContext context)
    {
        if (await context.Books.AnyAsync() || await context.Members.AnyAsync())
            return;

        var cleanCode = new Book("Clean Code", "Robert C. Martin", "9780132350884", 2008, 3);
        var pragmaticProgrammer = new Book("The Pragmatic Programmer", "Andrew Hunt", "9780135957059", 2019, 2);
        var domainDrivenDesign = new Book("Domain-Driven Design", "Eric Evans", "9780321125217", 2003, 2);
        var refactoring = new Book("Refactoring", "Martin Fowler", "9780134757599", 2018, 1);
        var designPatterns = new Book("Design Patterns", "Erich Gamma", "9780201633610", 1994, 4);

        var alice = new Member("Alice Johnson", "alice.johnson@example.com", "0771234567");
        var bob = new Member("Bob Smith", "bob.smith@example.com", "0777654321");
        var carol = new Member("Carol White", "carol.white@example.com", null);

        await context.Books.AddRangeAsync(cleanCode, pragmaticProgrammer, domainDrivenDesign, refactoring, designPatterns);
        await context.Members.AddRangeAsync(alice, bob, carol);
        await context.SaveChangesAsync();

        cleanCode.BorrowCopy();
        var activeBorrowing = new Borrowing(cleanCode.Id, alice.Id);

        refactoring.BorrowCopy();
        var secondBorrowing = new Borrowing(refactoring.Id, bob.Id);

        await context.Borrowings.AddRangeAsync(activeBorrowing, secondBorrowing);
        await context.SaveChangesAsync();
    }
}
