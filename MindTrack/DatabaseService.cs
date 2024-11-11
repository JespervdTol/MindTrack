using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MindTrack.Module
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            _connectionString = "Server=localhost;Database=mindtrack;User ID=root;Password=jtol123;Port=3306;";
        }

        public async Task<List<Person>> GetPersonDataByGameAsync(string game)
        {
            var personItems = new List<Person>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    SELECT p.id, p.name, p.birthday, s.game_score, s.date_played
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
                                Score = reader.IsDBNull("game_score") ? (int?)null : reader.GetInt32("game_score"),
                                DatePlayed = reader.IsDBNull("date_played") ? (DateTime?)null : reader.GetDateTime("date_played")
                            };
                            personItems.Add(item);
                        }
                    }
                }
            }

            return personItems;
        }

        public async Task<List<Person>> GetAllPersonsAsync()
        {
            var personItems = new List<Person>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT id, name, birthday FROM person";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var person = new Person
                            {
                                ID = reader.GetInt32("id"),
                                Name = reader.GetString("name"),
                                Birthday = reader.GetDateTime("birthday"),
                            };

                            personItems.Add(person);
                        }
                    }
                }
            }

            return personItems;
        }

        public async Task<bool> AddPersonService(string name, string email, DateTime birthday)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = "INSERT INTO person (name, email, birthday) VALUES (@name, @email, @birthday)";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@birthday", birthday);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting person into database: {ex.Message}");
                return false;
            }
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
}