using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Website.DAL.Entities
{
	// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
	public class ApplicationUser : IdentityUser
	{
		private IList<GameStatistic> _gameStatistic;

		public ApplicationUser()
		{
			_gameStatistic = new List<GameStatistic>();
		}

		[Required]
		[MaxLength(20)]
		public string DisplayName { get; set; }

		public virtual IList<GameStatistic> GameStatistics
		{
			get { return _gameStatistic; }
			set { _gameStatistic = value; }
		}

		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
		{
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

			// Add custom user claims here
			userIdentity.AddClaim(new Claim("UserIdClaimType", Id));

			return userIdentity;
		}
	}
}