using AuthService.Models;
using System.Threading.Tasks;

namespace AuthService.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task CreateUserAsync(User user);
    }
}
