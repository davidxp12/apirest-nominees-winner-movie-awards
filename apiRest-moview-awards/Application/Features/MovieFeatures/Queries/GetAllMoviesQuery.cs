using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Application.Features.MovieFeatures.Queries
{
	public class GetAllMoviesQuery : IRequest<IEnumerable<Movie>>
    {
        public class GetAllMoviesQueryHandler : IRequestHandler<GetAllMoviesQuery, IEnumerable<Movie>>
        {
            private readonly IMovieRepository<Movie> _movieRepository;
            public GetAllMoviesQueryHandler(IMovieRepository<Movie> repository)
            {
                _movieRepository = repository;
            }
            public async Task<IEnumerable<Movie>> Handle(GetAllMoviesQuery query, CancellationToken cancellationToken)
            {
                var moviesList = await _movieRepository.GetAllAsync(cancellationToken);
                if (moviesList == null)
                {
                    return null;
                }
                return moviesList.ToList();
            }
        }
    }
}
