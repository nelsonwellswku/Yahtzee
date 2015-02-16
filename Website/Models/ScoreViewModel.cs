using System;
using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
	public class ScoreViewModel
	{
		public string User { get; set; }
		public int Score { get; set; }

		[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
		public DateTime Date { get; set; }
	}
}