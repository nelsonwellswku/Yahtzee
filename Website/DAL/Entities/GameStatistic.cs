using System;

namespace Website.DAL.Entities
{
	public class GameStatistic
	{
		public int Id { get; set; }
		public virtual ApplicationUser User { get; set; }
		public virtual DateTime GameStartTime { get; set; }
		public virtual DateTime GameEndTime { get; set; }
		public virtual int FinalScore { get; set; }
		public virtual bool GameCompleted { get; set; }
	}
}