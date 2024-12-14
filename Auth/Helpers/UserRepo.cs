using Auth.Dal.Interfaces;
using Auth.Models;
using Auth.Models;
using Auth.Dal.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace Auth.Helpers;

public class UserRepo : IUserRepo
{
    private readonly string _connectionString = "Server=mathapp-tests.postgres.database.azure.com;Database=postgres;Port=5432;User Id=mathapp;Password=projektZespolowy123;Ssl Mode=Require;";


    public async Task<User?> GetOneById(string id)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand("SELECT * FROM User WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapReaderToUser(reader);
        }

        return null;
    }

    public async Task<User?> GetOneByMail(string mail)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand("SELECT * FROM User WHERE Mail = @Mail", connection);
        command.Parameters.AddWithValue("@Mail", mail);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapReaderToUser(reader);
        }

        return null;
    }

    public async Task<User> AddUser(User user)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand(
            "INSERT INTO User (Id, Mail, PasswordHash) VALUES (@Id, @Mail, @PasswordHash)", connection);
        command.Parameters.AddWithValue("@Id", user.Id);
        command.Parameters.AddWithValue("@Mail", user.Mail);
        command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);

        await command.ExecuteNonQueryAsync();
        return user;
    }

    public async Task DeleteUser(User user)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand("DELETE FROM User WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", user.Id);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<User> UpdateUser(User user)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand(
            "UPDATE User SET Mail = @Mail, PasswordHash = @PasswordHash WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", user.Id);
        command.Parameters.AddWithValue("@Mail", user.Mail);
        command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);

        await command.ExecuteNonQueryAsync();
        return user;
    }

    private User MapReaderToUser(SqlDataReader reader)
    {
        return new User
        {
            Id = reader["Id"].ToString(),
            Mail = reader["Email"].ToString(),
            PasswordHash = reader["PasswordHash"].ToString(),
        };
    }
}
