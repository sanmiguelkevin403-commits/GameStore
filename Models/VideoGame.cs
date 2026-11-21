namespace GameStore.Models;

public class VideoGame
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Genre { get; set; }
    public string Platform { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; }

    public VideoGame(int id, string name, string genre, string platform, decimal price, string status)
    {
        Id = id;
        Name = name;
        Genre = genre;
        Platform = platform;
        Price = price;
        Status = status;
    }

    public override string ToString() =>
        $"{Name} | {Genre} | {Platform} | ${Price:N2} | {Status}";
}
