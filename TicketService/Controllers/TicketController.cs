using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicketService.Models;
using TicketService.Repositories;

namespace TicketService.Controllers
{
    [ApiController]
    [Route("api/tickets")] // Dispatcher'ın bilet isteklerini beklediği adres
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketController(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        // 1. Bilet Satın Alma Uç Noktası
        [HttpPost("buy")]
        public async Task<IActionResult> BuyTicket([FromBody] Ticket ticket)
        {
            await _ticketRepository.BuyTicketAsync(ticket);

            return StatusCode(201, new { Message = "Bilet başarıyla satın alındı.", Ticket = ticket });
        }

        // 2. Kullanıcının Biletlerini Listeleme Uç Noktası
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetMyTickets(string userId)
        {
            var tickets = await _ticketRepository.GetTicketsByUserIdAsync(userId);

            return Ok(tickets);
        }
    }
}
