using EventService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventService.Repositories
{
    public interface IEventRepository
    {
        Task CreateEventAsync(Event newEvent);
        Task<IEnumerable<Event>> GetAllEventsAsync();
    }
}
