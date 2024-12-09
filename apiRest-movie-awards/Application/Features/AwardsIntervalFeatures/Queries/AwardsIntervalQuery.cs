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
				if (query == null || _movieRepository == null)
					throw new ArgumentNullException("Query ou repositório não podem ser nulos.");

				var listMovies = (await _movieRepository.GetAllAsync(cancellationToken))
					.Where(x => x.IsWinner())
					.ToList();

				if (!listMovies.Any())
					return null;

				// Extrair produtores e criar lista inicial de prêmios
				var producersWin = ExtractProducers(listMovies);

				// Agrupar intervalos por produtor
				var producerIntervals = CalculateIntervals(producersWin);

				// Montar resultado final
				var awardsIntervalDTO = new AwardsIntervalDTO
				{
					Min = producerIntervals.SelectMany(p => p.Min).OrderBy(x => x.Producer).ToList(),
					Max = producerIntervals.SelectMany(p => p.Max).OrderBy(x => x.Producer).ToList()
				};

				return awardsIntervalDTO;
			}

			private List<AwardsIntervalItemDTO> ExtractProducers(List<Movie> movies)
			{
				var producersWin = new List<AwardsIntervalItemDTO>();

				foreach (var movie in movies)
				{
					var producers = movie.Producers
						.Split(new[] { ", ", " and " }, StringSplitOptions.RemoveEmptyEntries);

					foreach (var producer in producers)
					{
						producersWin.Add(new AwardsIntervalItemDTO
						{
							Producer = producer,
							PreviousWin = movie.Year,
							FollowingWin = movie.Year
						});
					}
				}

				return producersWin;
			}

			private List<AwardsIntervalDTO> CalculateIntervals(List<AwardsIntervalItemDTO> producersWin)
			{
				// Agrupar por produtor
				var producerAwards = producersWin
					.GroupBy(d => d.Producer)
					.Select(g => new
					{
						Producer = g.Key,
						Years = g.Select(x => x.PreviousWin).OrderBy(y => y).ToList()
					})
					.ToList();

				// Calcular intervalos por produtor
				return producerAwards.Select(p =>
				{
					var intervals = CalculateProducerIntervals(p.Producer, p.Years);

					return new AwardsIntervalDTO
					{
						Min = intervals.Where(x => x.Interval == intervals.Min(i => i.Interval)).ToList(),
						Max = intervals.Where(x => x.Interval == intervals.Max(i => i.Interval)).ToList()
					};
				}).ToList();
			}

			private List<AwardsIntervalItemDTO> CalculateProducerIntervals(string producer, List<int> years)
			{
				if (years.Count < 2)
				{
					return new List<AwardsIntervalItemDTO>
					{
						new AwardsIntervalItemDTO
						{
							Producer = producer,
							PreviousWin = years.First(),
							FollowingWin = years.First()
						}
					};
				}

				return years.Zip(years.Skip(1), (previous, following) => new AwardsIntervalItemDTO
				{
					Producer = producer,
					PreviousWin = previous,
					FollowingWin = following
				}).ToList();
			}

		}
	}
}
