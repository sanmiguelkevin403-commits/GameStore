using Microsoft.Data.Sqlite;

namespace GameStore.Data;

public class GameDatabase
{
    private readonly string _connectionString;

    public GameDatabase()
    {
        string folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "GameStore");
        Directory.CreateDirectory(folder);
        string databasePath = Path.Combine(folder, "gamestore.db");
        _connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = databasePath
        }.ToString();
    }

    public SqliteConnection CreateConnection() => new(_connectionString);

    public void Initialize()
    {
        using var connection = CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
            CREATE TABLE IF NOT EXISTS VideoGames (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Genre TEXT NOT NULL,
                Platform TEXT NOT NULL,
                Price REAL NOT NULL,
                Status TEXT NOT NULL
            );
            """;
        command.ExecuteNonQuery();
    }
}
