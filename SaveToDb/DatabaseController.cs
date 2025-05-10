using Npgsql;
using dotenv.net;

public class DatabaseController
{
    private string? _connectionString;

    public async Task InitializeAsync()
    {
        DotEnv.Load(); // Load .env file
        _connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
        ?? throw new Exception("DATABASE_URL environment variable is not set");;
        
        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new ArgumentNullException("DATABASE_URL environment variable is not set");
        }
        
        // Verify connection and create table
        await VerifyConnectionAsync();
        await EnsureTableExists();
    }

    private async Task VerifyConnectionAsync()
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to connect to database: {ex.Message}");
        }
    }

    private async Task EnsureTableExists()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        // Read the schema file
        string schemaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Databases", "Schema.sql");
        string createTableQuery = await File.ReadAllTextAsync(schemaPath);

        using var command = new NpgsqlCommand(createTableQuery, connection);
        await command.ExecuteNonQueryAsync();
    }

    public async Task SaveEmailAsync(string email, string userId)
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = "INSERT INTO users (CHAT_ID, EMAIL, CREATED_DATE) VALUES (@UserId, @Email, @CreatedAt)";

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);

            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            // Log the exception
            throw new Exception($"Error saving email to database: {ex.Message}");
        }
    }
}