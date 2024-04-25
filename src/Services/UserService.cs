namespace backend.Services;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

public class UserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> RegisterUser(string username, string password)
    {
        var user = new User { Username = username, PasswordHash = BCrypt.Net.BCrypt.HashPassword(password) };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> AuthenticateUser(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null) return null;

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) return null;

        return user;
    }

    public List<User> GetAllUsers()
    {
        var users = _context.Users.ToList();
        if (users == null) return null;
        return users;
    }

    public User GetByUsername(string username)
    {
        var user = _context.Users.FirstOrDefault(user => user.Username == username);
        return (user == null)? null : user;
    }
}
