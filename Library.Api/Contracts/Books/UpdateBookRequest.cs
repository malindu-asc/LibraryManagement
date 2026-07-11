using System.ComponentModel.DataAnnotations;

namespace Library.Api.Contracts.Books;

public class UpdateBookRequest
{
    [Required]
    public string Title { get; set; } = default!;

    [Required]
    public string Author { get; set; } = default!;

    [Required]
    public string Isbn { get; set; } = default!;

    [Required]
    public int PublishedYear { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "TotalCopies must be greater than 0.")]
    public int TotalCopies { get; set; }
}
