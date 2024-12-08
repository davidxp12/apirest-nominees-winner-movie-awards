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
	public class AwardsIntervalQuery : IRequest<AwardsIntervalDTO>
	{
		public class AwardsIntervalQueryHandler : IRequestHandler<AwardsIntervalQuery, AwardsIntervalDTO>
		{
			private readonly IMovieRepository<Movie> _movieRepository;
			public AwardsIntervalQueryHandler(IMovieRepository<Movie> repository)
			{
				_movieRepository = repository;
			}
			public async Task<AwardsIntervalDTO> Handle(AwardsIntervalQuery query, CancellationToken cancellationToken)
			{
				AwardsIntervalDTO awardsIntervalDTO = new AwardsIntervalDTO();

				var listMovies = _movieRepository.GetAll().Where(x => x.IsWinner() == true);

				if (listMovies == null)
				{
					return null;
				}

				List<AwardsIntervalItemDTO> producersWin = new List<AwardsIntervalItemDTO>();

				foreach (var movie in listMovies)
				{
					var producers = movie.Producers.Split(new[] { ", ", " and " }, StringSplitOptions.RemoveEmptyEntries).ToList();

					foreach (var producer in producers)
					{
						producersWin.Add(new AwardsIntervalItemDTO()
						{
							Producer = producer,
							PreviousWin = movie.Year,
							FollowingWin = movie.Year
						});
					}
				}

				// Agrupar por producer
				var producerAwards = producersWin
					.GroupBy(d => d.Producer)
					.Select(g => new
					{
						Producer = g.Key,
						Years = g.Select(x => x.PreviousWin).OrderBy(y => y).ToList()
					})
					.ToList();

				// Calcular os intervalos para cada producer
				//var producerIntervals = producerAwards
				//	.SelectMany(p => p.Years.Zip(p.Years.Skip(1), (previous, following) => new
				//	{
				//		Producer = p.Producer,
				//		Interval = following - previous,
				//		PreviousWin = previous,
				//		FollowingWin = following
				//	}))
				//	.ToList();

				var producerIntervals = producerAwards
				.SelectMany(p =>
				{
					// Se o produtor tiver apenas um prêmio, adicionamos uma entrada com intervalo 0
					if (p.Years.Count < 2)
					{
						return new List<AwardsIntervalItemDTO>
						{
							new AwardsIntervalItemDTO
							{
								Producer = p.Producer,
								PreviousWin = p.Years.First(),
								FollowingWin = p.Years.First()
							}
						};
					}

					// Para produtores com mais de um prêmio, calculamos os intervalos normalmente
					return p.Years.Zip(p.Years.Skip(1), (previous, following) => new AwardsIntervalItemDTO
					{
						Producer = p.Producer,
						PreviousWin = previous,
						FollowingWin = following
					});
				})
				.ToList();

				// Encontrar os intervalos mínimos
				int minIntervalValue = producerIntervals.Where(p => p.Interval > 0).Min(p => p.Interval);
				var minIntervals = producerIntervals
					.Where(p => p.Interval <= minIntervalValue)
					.ToList();

				// Encontrar os intervalos máximos
				int maxIntervalValue = producerIntervals.Where(p => p.Interval > 0).Max(p => p.Interval);
				var maxIntervals = producerIntervals
					.Where(p => p.Interval >= maxIntervalValue)
					.ToList();

				awardsIntervalDTO.Min = minIntervals?.Select(p => new AwardsIntervalItemDTO { Producer = p.Producer, PreviousWin = p.PreviousWin, FollowingWin = p.FollowingWin }).ToList();
				 awardsIntervalDTO.Max = maxIntervals?.Select(p => new AwardsIntervalItemDTO { Producer = p.Producer, PreviousWin = p.PreviousWin, FollowingWin = p.FollowingWin }).ToList();

				//awardsIntervalDTO.Min = minInterval?.SelectMany(p => new AwardsIntervalItemDTO { Producer = p.Producer, PreviousWin = p.PreviousWin, FollowingWin = p.FollowingWin }).ToList();
				//awardsIntervalDTO.Max = maxInterval?.SelectMany(p => new AwardsIntervalItemDTO { Producer = p.Producer, PreviousWin = p.PreviousWin, FollowingWin = p.FollowingWin }).ToList();

				return awardsIntervalDTO;
			}
		}
	}
}
