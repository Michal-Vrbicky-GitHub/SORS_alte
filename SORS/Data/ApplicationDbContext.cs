using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SORS.Data.Models;

namespace SORS.Data
{
	public class ApplicationDbContext : IdentityDbContext//<IdentityUser>
	{
		public DbSet<Station> Stations { get; set; }
		public DbSet<AlertEmail> AlertEmails { get; set; }
        public DbSet<StationReport> Report { get; set; }
        //public DbSet<Token> UserTokens { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Station>()
				.HasMany(s => s.AlertEmails)
				.WithOne(ae => ae.Station)
				.HasForeignKey(ae => ae.StationID);
		}
	}
}