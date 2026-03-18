using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using EventService.Models;
using EventService.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventService.Controllers
{
    [ApiController]
    [Route("api/events")] // Gateway (Dispatcher) istekleri tam olarak buraya atacak
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;

        // 1. Veritabaný Baðlantýsýný Ýçeri Alma (Dependency Injection)
        public EventController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        // 2. Tüm Etkinlikleri Getirme Uç Noktasý (GET Ýstekleri)
        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _eventRepository.GetAllEventsAsync();
            return Ok(events); // Baþarýlý olursa verilerle birlikte 200 OK döner
        }

        // 3. Yeni Etkinlik Ekleme Uç Noktasý (POST Ýstekleri)
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] Event newEvent)
        {
            await _eventRepository.CreateEventAsync(newEvent);

            // Baþarýyla kaydedildiðinde 201 Created döner
            return StatusCode(201, new { Message = "Etkinlik baþarýyla eklendi.", Event = newEvent });
        }
    }
}
