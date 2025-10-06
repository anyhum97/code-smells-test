using Microsoft.AspNetCore.Mvc;

using code_smells_test.Services.Interfaces;
using code_smells_test.Repositories.Interfaces;
using code_smells_test.Models;

namespace code_smells_test.Controllers
{
	[ApiController]
	[Route("api/tickets")]
	public class TicketsController : ControllerBase
	{
		private readonly ITicketRepository _ticketRepository;
		private readonly ITimeZoneService _timeZoneService;

		public TicketsController(ITicketRepository ticketRepository, ITimeZoneService timeZoneService)
		{
			_ticketRepository = ticketRepository;
			_timeZoneService = timeZoneService;
		}

		[HttpGet("all")]
		public async Task<IActionResult>GetAllTickets()
		{
			var tickets = await _ticketRepository.GetAllTicketsAsync();

			return Ok(tickets);
		}

		[HttpPost]
		public async Task<IActionResult> AddTicket([FromBody] Ticket ticket)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			ArgumentException.ThrowIfNullOrWhiteSpace(ticket.Title);

			// Конвертируем локальную дату клиента в UTC перед сохранением
			ticket.VisitDate = _timeZoneService.ConvertToUtc(ticket.VisitDate);
			ticket.CreationDate = DateTime.UtcNow;

			if(ticket.VisitDate < DateTime.UtcNow)
			{
				throw new Exception("Ticket creation in the past is disabled");
			}

			await _ticketRepository.AddTicketAsync(ticket);

			return StatusCode(201, ticket);
		}
	}
}
