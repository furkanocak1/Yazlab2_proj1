using EventService.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventService.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly IMongoCollection<Event> _events;

        // Arkadaşının yaptığı gibi IConfiguration ile appsettings.json'ı okuyoruz
        public EventRepository(IConfiguration config)
        {
            var mongoClient = new MongoClient(config.GetSection("MongoDbSettings:ConnectionString").Value);
            var mongoDatabase = mongoClient.GetDatabase(config.GetSection("MongoDbSettings:DatabaseName").Value);
            _events = mongoDatabase.GetCollection<Event>(config.GetSection("MongoDbSettings:CollectionName").Value);
        }

        public async Task CreateEventAsync(Event newEvent)
        {
            await _events.InsertOneAsync(newEvent);
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _events.Find(_ => true).ToListAsync();
        }
    }
}