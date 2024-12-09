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
    public class GetMovieByTitleQuery : IRequest<IEnumerable<Movie>>
    {
        public string Title { get; set; }
        public class GetMovieByTitleHandler : IRequestHandler<GetMovieByTitleQuery, IEnumerable<Movie>>
        {
            private readonly IMovieRepository<Movie> _movieRepository;
            public GetMovieByTitleHandler(IMovieRepository<Movie> repository)
            {
                _movieRepository = repository;
            }
            public async Task<IEnumerable<Movie>> Handle(GetMovieByTitleQuery query, CancellationToken cancellationToken)
            {
                var moviesList = _movieRepository.GetByTitle(query.Title);
                if (moviesList == null)
                {
                    return null;
                }
                return moviesList.ToList();
            }
        }
    }
}