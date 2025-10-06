using code_smells_test.Models;

namespace code_smells_test.Repositories.Interfaces
{
	public interface ITicketRepository
	{
		public Task<IEnumerable<Ticket>> GetAllTicketsAsync();

		public Task AddTicketAsync(Ticket ticket);
	}
}
