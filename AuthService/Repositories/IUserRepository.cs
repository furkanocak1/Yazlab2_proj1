using AuthService.Models;
using System.Threading.Tasks;

namespace AuthService.Repositories
{
    
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email); // Email'e göre kullanýcýyý bul
        Task CreateUserAsync(User user); // Yeni kullanýcýyý veritabanýna kaydet
    }
}