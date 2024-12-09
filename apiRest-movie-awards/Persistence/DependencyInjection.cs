

using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static void AddPersistence(this IServiceCollection services)
        {
            services.AddTransient<Application.Interfaces.IMovieRepository<Movie>, Persistence.Repository.IMovieRepository<Movie>>();
        }
    }
}
