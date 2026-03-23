using System.Collections.Generic;
using System.Threading.Tasks;
using TicketService.Models;

namespace TicketService.Repositories
{
    public interface ITicketRepository
    {
        Task BuyTicketAsync(Ticket ticket);
        Task<IEnumerable<Ticket>> GetTicketsByUserIdAsync(string userId);
    }
}