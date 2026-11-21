using GameStore.Models;
using Microsoft.Data.Sqlite;

namespace GameStore.Data;

public class GameRepository
{
    private readonly GameDatabase _database;

    public GameRepository(GameDatabase database)
    {
        _database = database;
    }

    public List<VideoGame> GetAll()
    {
        var games = new List<VideoGame>();
        using var connection = _database.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT Id, Name, Genre, Platform, Price, Status
            FROM VideoGames ORDER BY Name COLLATE NOCASE;
            """;

        using SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            games.Add(new VideoGame(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                Convert.ToDecimal(reader.GetDouble(4)),
                reader.GetString(5)));
        }
        return games;
    }

    public void Add(VideoGame game)
    {
        using var connection = _database.CreateConnection();
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO VideoGames (Name, Genre, Platform, Price, Status)
            VALUES ($name, $genre, $platform, $price, $status);
            """;
        AddParameters(command, game);
        command.ExecuteNonQuery();
    }

    public void Update(VideoGame game)
    {
        using var connection = _database.CreateConnection();
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = """
            UPDATE VideoGames
            SET Name=$name, Genre=$genre, Platform=$platform, Price=$price, Status=$status
            WHERE Id=$id;
            """;
        AddParameters(command, game);
        command.Parameters.AddWithValue("$id", game.Id);
        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = _database.CreateConnection();
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM VideoGames WHERE Id=$id;";
        command.Parameters.AddWithValue("$id", id);
        command.ExecuteNonQuery();
    }

    private static void AddParameters(SqliteCommand command, VideoGame game)
    {
        command.Parameters.AddWithValue("$name", game.Name);
        command.Parameters.AddWithValue("$genre", game.Genre);
        command.Parameters.AddWithValue("$platform", game.Platform);
        command.Parameters.AddWithValue("$price", game.Price);
        command.Parameters.AddWithValue("$status", game.Status);
    }
}
