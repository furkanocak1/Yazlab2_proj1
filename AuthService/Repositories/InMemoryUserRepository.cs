using System.Collections.Concurrent;
using AuthService.Models;
using MongoDB.Bson;

namespace AuthService.Repositories;

public class InMemoryUserRepository : IUserRepository
{
    private readonly ConcurrentDictionary<string, User> _byEmail = new(StringComparer.OrdinalIgnoreCase);

    public Task CreateUserAsync(User user)
    {
        user.Id ??= ObjectId.GenerateNewId().ToString();
        if (!_byEmail.TryAdd(user.Email, user))
            throw new InvalidOperationException("duplicate");
        return Task.CompletedTask;
    }

    public Task<User?> GetUserByEmailAsync(string email)
    {
        if (string.IsNullOrEmpty(email))
            return Task.FromResult<User?>(null);
        _byEmail.TryGetValue(email, out var u);
        return Task.FromResult(u);
    }
}
