using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.MovieFeatures
{
	public class CreateMovieCommand : IRequest<bool>
    {
		public Movie Movie { get; set; }
		public class CreateProductCommandHandler : IRequestHandler<CreateMovieCommand, bool>
        {
            private readonly IMovieRepository<Movie> _movieRepository;
            public CreateProductCommandHandler(IMovieRepository<Movie> repository)
            {
                _movieRepository = repository;
            }

            public async Task<bool> Handle(CreateMovieCommand command, CancellationToken cancellationToken)
            {
                //TO DO VALIDATION FIELDS

                _movieRepository.Add(command.Movie);

                return true;
            }
        }
    }
}
