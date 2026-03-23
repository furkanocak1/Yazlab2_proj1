using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketService.Models;

namespace TicketService.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly IMongoCollection<Ticket> _tickets;

        public TicketRepository(IConfiguration config)
        {
            var mongoClient = new MongoClient(config.GetSection("MongoDbSettings:ConnectionString").Value);
            var mongoDatabase = mongoClient.GetDatabase(config.GetSection("MongoDbSettings:DatabaseName").Value);
            _tickets = mongoDatabase.GetCollection<Ticket>(config.GetSection("MongoDbSettings:CollectionName").Value);
        }

        public async Task BuyTicketAsync(Ticket ticket)
        {
            await _tickets.InsertOneAsync(ticket);
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByUserIdAsync(string userId)
        {
            // Sadece o kullanıcıya ait biletleri getir
            return await _tickets.Find(t => t.UserId == userId).ToListAsync();
        }
    }
}