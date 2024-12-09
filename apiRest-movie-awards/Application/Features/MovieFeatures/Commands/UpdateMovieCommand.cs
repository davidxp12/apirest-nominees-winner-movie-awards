using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.MovieFeatures.Commands
{
	public class UpdateMovieCommand : IRequest<bool>
    {
        public Movie Movie { get; set; }
        public class CreateProductCommandHandler : IRequestHandler<UpdateMovieCommand, bool>
        {
            private readonly IMovieRepository<Movie> _movieRepository;
            public CreateProductCommandHandler(IMovieRepository<Movie> repository)
            {
                _movieRepository = repository;
            }

            public async Task<bool> Handle(UpdateMovieCommand command, CancellationToken cancellationToken)
            {
                //TO DO VALIDATION FIELDS

                var movie = _movieRepository.GetByTitle(command.Movie.Title);

                if (movie == null)
                {
                    return default;
                }
                else
                {
                    _movieRepository.Update(command.Movie);
                }

                return true;
            }
        }
    }
}