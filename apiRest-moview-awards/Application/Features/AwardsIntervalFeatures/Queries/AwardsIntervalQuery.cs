using Application.Interfaces;
using Domain.DTO;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Application.Features.AwardsIntervalFeatures.Queries
{
	public class AwardsIntervalQuery : IRequest<IEnumerable<AwardsIntervalDTO>>
	{
		public bool? Winner { get; set; }
		public class AwardsIntervalQueryHandler : IRequestHandler<AwardsIntervalQuery, IEnumerable<AwardsIntervalDTO>>
		{
			private readonly IMovieRepository<Movie> _movieRepository;
			public AwardsIntervalQueryHandler(IMovieRepository<Movie> repository)
			{
				_movieRepository = repository;
			}
			public async Task<IEnumerable<AwardsIntervalDTO>> Handle(AwardsIntervalQuery query, CancellationToken cancellationToken)
			{
				List<AwardsIntervalDTO> awardsIntervalDTO = new List<AwardsIntervalDTO>();

				var listMovies = _movieRepository.GetAll().Where(x => x.IsWinner() == (query.Winner is null ? true : query.Winner));

				if (listMovies == null)
				{
					return null;
				}


				return awardsIntervalDTO;
			}
		}
	}
}
