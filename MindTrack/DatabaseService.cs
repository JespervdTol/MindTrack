using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MindTrack.Module;
using MySql.Data.MySqlClient;

public class DatabaseService
{
    private readonly string _connectionString;

    public DatabaseService()
    {
        _connectionString = "Server=192.168.153.146;Database=mindtrack;User ID=admin;Password=B9J6-vw#fk%qHkrAB9*j;Port=3306;";
    }

    public async Task<List<Person>> GetPersonDataByGameAsync(string game)
    {
        var personItems = new List<Person>();

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"
                SELECT p.id, p.name, p.birthday, s.game_score 
                FROM person p
                LEFT JOIN score s ON p.id = s.person_id
                LEFT JOIN game g ON s.game_id = g.id
                WHERE g.name = @game";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@game", game);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var item = new Person
                        {
                            ID = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Birthday = reader.GetDateTime("birthday"),
                            Score = reader.IsDBNull("game_score") ? (int?)null : reader.GetInt32("game_score")
                        };
                        personItems.Add(item);
                    }
                }
            }
        }

        return personItems;
    }

    public async Task<List<string>> GetGamesAsync()
    {
        var gameList = new List<string>();

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = "SELECT name FROM game";
            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        gameList.Add(reader.GetString("name"));
                    }
                }
            }
        }

        return gameList;
    }
}