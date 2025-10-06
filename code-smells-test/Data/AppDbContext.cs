using Microsoft.EntityFrameworkCore;

using code_smells_test.Models;

namespace code_smells_test.Data
{
	public class AppDbContext : DbContext
    {
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
			
        }

		public DbSet<Ticket> Tickets { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Ticket>().ToTable("tickets");

			modelBuilder.Entity<Ticket>()
				.Property(u => u.Id)
				.HasColumnName("id")
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<Ticket>()
				.Property(u => u.CreationDate)
				.HasColumnName("created");

			modelBuilder.Entity<Ticket>()
				.Property(u => u.Title)
				.HasColumnName("title")
				.HasMaxLength(200)
				.IsRequired();

			modelBuilder.Entity<Ticket>()
				.Property(u => u.Description)
				.HasColumnName("description")
				.HasMaxLength(1000);				

			modelBuilder.Entity<Ticket>()
				.Property(u => u.VisitDate)
				.HasColumnName("visited");

			modelBuilder.Entity<Ticket>()
				.Property(u => u.VisitorsNumber)
				.HasColumnName("number");
		}
    }
}
