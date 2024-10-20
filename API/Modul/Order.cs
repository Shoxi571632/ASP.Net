namespace API.Modul;
using System;

public class Order
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public Book Book { get; set; }
    public User User { get; set; }
}
