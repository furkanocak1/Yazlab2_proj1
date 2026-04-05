using AuthService.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace AuthService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        // Sýnýf baþlarken appsettings.json'daki ayarlarý okuyup MongoDB'ye baðlanýyoruz
        public UserRepository(IConfiguration config)
        {
            var mongoClient = new MongoClient(config.GetSection("MongoDbSettings:ConnectionString").Value);
            var mongoDatabase = mongoClient.GetDatabase(config.GetSection("MongoDbSettings:DatabaseName").Value);
            _users = mongoDatabase.GetCollection<User>(config.GetSection("MongoDbSettings:CollectionName").Value);
        }

        // Yeni kullanýcýyý kaydetme komutu
        public async Task CreateUserAsync(User user)
        {
            await _users.InsertOneAsync(user);
        }

        // Email'e göre kullanýcýyý getirme komutu (Giriþ yaparken lazým olacak)
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _users.Find(x => x.Email == email).FirstOrDefaultAsync();
        }
    }
}