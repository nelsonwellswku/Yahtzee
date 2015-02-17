using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using Website.DAL.Entities;

namespace Website.DAL
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext()
			: base("DefaultConnection", false)
		{
		}

		public static ApplicationDbContext Create()
		{
			return new ApplicationDbContext();
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			modelBuilder.Entity<ApplicationUser>()
				.Property(x => x.DisplayName)
				.HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_DisplayName") { IsUnique = true}));
			modelBuilder.Entity<GameStatistic>();

			base.OnModelCreating(modelBuilder);
		}
	}
}