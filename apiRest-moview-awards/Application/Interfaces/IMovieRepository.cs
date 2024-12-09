using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces
{
	public interface IMovieRepository<T>
	{
		void Add(T movie);
		void AddBulkInsert(List<T> movies);
		public IEnumerable<T> GetAll();
		public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
		public IEnumerable<T> GetByTitle(string title);
		public void Remove(string title);
		public void Update(T movie);
	}
}
