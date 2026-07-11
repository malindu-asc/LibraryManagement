namespace Library.Api.Domain.Entities;

public class Book
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = default!;
    public string Author { get; private set; } = default!;
    public string Isbn { get; private set; } = default!;
    public int PublishedYear { get; private set; }
    public int TotalCopies { get; private set; }
    public int AvailableCopies { get; private set; }

    private Book() { }

    public Book(string title, string author, string isbn, int publishedYear, int totalCopies)
    {
        Id = Guid.NewGuid();
        Title = title;
        Author = author;
        Isbn = isbn;
        PublishedYear = publishedYear;
        TotalCopies = totalCopies;
        AvailableCopies = totalCopies;
    }

    public void Update(string title, string author, string isbn, int publishedYear, int totalCopies)
    {
        var borrowedCopies = TotalCopies - AvailableCopies;
        if (totalCopies < borrowedCopies)
            throw new InvalidOperationException("TotalCopies cannot be less than currently borrowed copies.");

        Title = title;
        Author = author;
        Isbn = isbn;
        PublishedYear = publishedYear;
        TotalCopies = totalCopies;
        AvailableCopies = totalCopies - borrowedCopies;
    }

    public void BorrowCopy()
    {
        if (AvailableCopies <= 0)
            throw new InvalidOperationException("No available copies to borrow.");
        AvailableCopies--;
    }

    public void ReturnCopy()
    {
        if (AvailableCopies >= TotalCopies)
            throw new InvalidOperationException("All copies are already accounted for.");
        AvailableCopies++;
    }
}
