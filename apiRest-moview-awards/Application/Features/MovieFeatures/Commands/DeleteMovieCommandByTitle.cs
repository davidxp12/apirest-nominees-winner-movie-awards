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
	public class DeleteMovieCommandByTitle : IRequest<bool>
    {
		public string Title { get; set; }
		public class DeleteMovieCommandHandle : IRequestHandler<DeleteMovieCommandByTitle, bool>
        {
            private readonly IMovieRepository<Movie> _movieRepository;
            public DeleteMovieCommandHandle(IMovieRepository<Movie> repository)
            {
                _movieRepository = repository;
            }

            public async Task<bool> Handle(DeleteMovieCommandByTitle command, CancellationToken cancellationToken)
            {
                //TO DO VALIDATION FIELDS

                _movieRepository.Remove(command.Title);

                return true;
            }
        }
    }
}

