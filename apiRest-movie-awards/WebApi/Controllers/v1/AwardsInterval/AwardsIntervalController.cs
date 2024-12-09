using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.AwardsIntervalFeatures.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiRestMovieAwards.BFF.Controllers.v1
{
	[ApiVersion("1.0")]
	public class AwardsIntervalController : BaseApiController
	{
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			return Ok(await Mediator.Send(new AwardsIntervalQuery() {}));
		}
	}
}
