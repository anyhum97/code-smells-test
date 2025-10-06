using Microsoft.AspNetCore.Mvc;

using code_smells_test.Repositories.Interfaces;
using code_smells_test.Models;

namespace code_smells_test.Controllers
{
	[ApiController]
	[Route("api/tickets")]
	public class TicketsController : ControllerBase
	{
		private readonly ITicketRepository _ticketRepository;

		public TicketsController(ITicketRepository ticketRepository)
		{
			_ticketRepository = ticketRepository;
		}

		[HttpGet("all")]
		public async Task<IActionResult>GetAllTickets()
		{
			var tickets = await _ticketRepository.GetAllTicketsAsync();

			return Ok(tickets);
		}

		[HttpPost]
		public async Task<IActionResult>AddTicket([FromBody] Ticket ticket)
		{
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			ArgumentException.ThrowIfNullOrWhiteSpace(ticket.Title);

			if(ticket.VisitDate < DateTime.UtcNow)
            {
                throw new Exception("Ticket creation in the past is disabled");
            }

			await _ticketRepository.AddTicketAsync(ticket);

			return StatusCode(201, ticket);
		}
	}
}
