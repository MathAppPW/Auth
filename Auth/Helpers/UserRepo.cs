using Auth.Dal.Interfaces;
using Auth.Models;
using Auth.Models;
using Auth.Dal.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace Auth.Helpers;

public class UserRepo : IUserRepo
{
    private readonly AppDbContext _context;

    public UserRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetOneById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("GetOneById() received empty id string", nameof(id));
        }

        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new InvalidOperationException($"User with id {id} not found.");
        }

        return user;
    }

    public async Task<User?> GetOneByMail(string mail)
    {
        if (string.IsNullOrWhiteSpace(mail))
        {
            throw new ArgumentException("GetOneByMail() received empty mail string", nameof(mail));
        }

        return await _context.Users.FirstOrDefaultAsync(u => u.Mail == mail);
    }

    public async Task<User> AddUser(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task DeleteUser(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> UpdateUser(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var existingUser = await _context.Users.FindAsync(user.Id);
        if (existingUser == null)
        {
            throw new InvalidOperationException($"User with id {user.Id} not found.");
        }

        existingUser.Mail = user.Mail;
        existingUser.PasswordHash = user.PasswordHash;

        _context.Users.Update(existingUser);
        await _context.SaveChangesAsync();

        return existingUser;
    }
}
