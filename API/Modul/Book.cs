namespace API.Modul;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty; // Kerakli maydon
    public int AuthorId { get; set; }
    public int GenreId { get; set; }
    public Author? Author { get; set; }
    public Genre? Genre { get; set; }
    public ICollection<Order>? Orders { get; set; }
}
