using Microsoft.EntityFrameworkCore;

using code_smells_test.Data;
using code_smells_test.Repositories.Interfaces;
using code_smells_test.Models;

namespace code_smells_test.Repositories
{
	public class TicketRepository : ITicketRepository
	{
		private readonly AppDbContext _context;

		public TicketRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
		{
			return await _context.Tickets.ToListAsync();
		}

		public async Task AddTicketAsync(Ticket ticket)
		{
			ArgumentNullException.ThrowIfNull(ticket);

			await _context.AddAsync(ticket);

			await _context.SaveChangesAsync();
		}
	}
}
