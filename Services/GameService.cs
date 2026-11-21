using GameStore.Data;
using GameStore.Models;

namespace GameStore.Services;

public class GameService
{
    private readonly GameRepository _repository;

    public GameService(GameRepository repository)
    {
        _repository = repository;
    }

    public List<VideoGame> GetGames() => _repository.GetAll();

    public void AddGame(string name, string genre, string platform, string priceText, string status)
    {
        Validate(name, genre, platform, priceText, status, out decimal price);
        _repository.Add(new VideoGame(0, name.Trim(), genre.Trim(), platform.Trim(), price, status));
    }

    public void UpdateGame(int id, string name, string genre, string platform, string priceText, string status)
    {
        if (id <= 0) throw new ArgumentException("Selecciona un videojuego válido.");
        Validate(name, genre, platform, priceText, status, out decimal price);
        _repository.Update(new VideoGame(id, name.Trim(), genre.Trim(), platform.Trim(), price, status));
    }

    public void DeleteGame(int id)
    {
        if (id <= 0) throw new ArgumentException("Selecciona un videojuego para eliminar.");
        _repository.Delete(id);
    }

    private static void Validate(
        string name, string genre, string platform, string priceText, string status,
        out decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Escribe el nombre del videojuego.");
        if (string.IsNullOrWhiteSpace(genre))
            throw new ArgumentException("Escribe el género.");
        if (string.IsNullOrWhiteSpace(platform))
            throw new ArgumentException("Escribe la plataforma.");
        if (!decimal.TryParse(priceText, System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.CurrentCulture, out price) || price < 0)
            throw new ArgumentException("El precio debe ser un número válido mayor o igual a cero.");
        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("Escribe el estado del videojuego.");
    }
}
