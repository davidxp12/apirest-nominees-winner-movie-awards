using Application.Interfaces;
using Domain.Entities;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repository
{
	public class IMovieRepository<T> : Application.Interfaces.IMovieRepository<T> where T : Movie
	{
		private readonly ILiteDatabase _liteDatabase;
		private ILiteCollection<T> _liteCollection { get { return _liteDatabase.GetCollection<T>(typeof(T).Name); } }

		public IMovieRepository(ILiteDatabase liteDatabase)
		{
			_liteDatabase = liteDatabase;
			_liteCollection.EnsureIndex(x => x.Title);
		}

		public void Add(T movie)
		{
			_liteCollection.Insert(movie);
		}
		public void AddBulkInsert(List<T> movies)
		{
			_liteCollection.InsertBulk(movies);
		}

		public IEnumerable<T> GetAll()
		{
			return _liteCollection.FindAll();
		}

		public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
		{
			return await Task.Run(() =>
			{
				cancellationToken.ThrowIfCancellationRequested();

				var result = _liteCollection.FindAll();

				cancellationToken.ThrowIfCancellationRequested();

				return result;
			}, cancellationToken);
		}

		public IEnumerable<T> GetByTitle(string title)
		{
			return _liteCollection.Find(x => x.Title.ToUpper() == title.ToUpper());
		}

		public void Remove(string title)
		{
			_liteCollection.DeleteMany(x => x.Title == title);
		}

		public void Update(T movie)
		{
			var movieToUpdate = _liteCollection.FindOne(x => x.Title.ToUpper() == movie.Title.ToUpper());
			if (movieToUpdate != null)
			{
				_liteCollection.Update(movie);
			}
		}
	}
}
