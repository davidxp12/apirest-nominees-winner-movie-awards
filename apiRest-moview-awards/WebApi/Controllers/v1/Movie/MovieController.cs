using Application.Features.AwardsIntervalFeatures.Queries;
using Application.Features.MovieFeatures;
using Application.Features.MovieFeatures.Commands;
using Application.Features.MovieFeatures.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestMovieAwards.BFF.Controllers.v1
{
	[ApiVersion("1.0")]
	public class MovieController : BaseApiController
	{
		[HttpPost]
		public async Task<IActionResult> Create(CreateMovieCommand command)
		{
			return Ok(await Mediator.Send(command));
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			return Ok(await Mediator.Send(new GetAllMoviesQuery()));
		}

		[HttpGet("{title}")]
		public async Task<IActionResult> GetById(string title)
		{
			return Ok(await Mediator.Send(new GetMovieByTitleQuery { Title = title }));
		}

		[HttpDelete("{title}")]
		public async Task<IActionResult> Delete(string title)
		{
			return Ok(await Mediator.Send(new DeleteMovieCommandByTitle { Title = title }));
		}

		[HttpPut("[action]")]
		public async Task<IActionResult> Update(UpdateMovieCommand command)
		{
			if (string.IsNullOrEmpty(command?.Movie?.Title))
			{
				return BadRequest();
			}
			return Ok(await Mediator.Send(command));
		}
	}
}
