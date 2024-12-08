using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
	public class AwardsIntervalDTO
	{
		public List<AwardsIntervalItemDTO> Min { get; set; }
		public List<AwardsIntervalItemDTO> Max { get; set; }
	}

	public class AwardsIntervalItemDTO
	{
		public string Producer { get; set; }
		public int Interval { get; set; }
		public int PreviousWin { get; set; }
		public int FollowingWin { get; set; }
	}
}
