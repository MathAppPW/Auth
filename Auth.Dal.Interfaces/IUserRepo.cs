using Auth.Models;

namespace Auth.Dal.Interfaces;

public interface IUserRepo
{
    Task<User?> GetOneById(string id);
    Task<User?> GetOneByMail(string mail);
    Task<User> AddUser(User user);
    Task DeleteUser(User user);
    Task<User> UpdateUser(User user);
}