using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
	public class Movie
	{
		[Name("title")]
		public string Title { get; set; }
		[Name("year")]
		public int Year { get; set; }
		[Name("studios")]
		public string Studios { get; set; }
		[Name("producers")]
		public string Producers { get; set; }
		[Name("winner")]
		public string Winner { get; set; }

		public bool IsWinner() => (Winner == "yes");
	}
}
