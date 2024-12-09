
using Application.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using Domain.Entities;
using Persistence.ReaderFile.Interface;
using Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.ReaderFile.csv
{
	public class ReaderCsvMoviesService : IReaderCsvMoviesService
	{
		private readonly Application.Interfaces.IMovieRepository<Movie> _movieRepository;
		public ReaderCsvMoviesService(Application.Interfaces.IMovieRepository<Movie> movieRepository)
		{
			_movieRepository = movieRepository;
		}
		public static CsvConfiguration CsvConfiguration
		{
			get
			{
				return new CsvConfiguration(CultureInfo.InvariantCulture)
				{
					HasHeaderRecord = true,
					MissingFieldFound = null,
					BadDataFound = null,
					//HeaderValidated = null,
					//MemberTypes = CsvHelper.Configuration.MemberTypes.Fields,
					Delimiter = ";",
					//Quote = '\''
				};
			}
		}

		public async Task RunAsync()
		{
			Console.WriteLine("Serviço inicializado com sucesso!");

			List<Movie> movieList = GetMoviesFile();

			SaveDbMovies(movieList);

			Console.WriteLine("Serviço finalizado com sucesso!");

			await Task.Delay(1);
		}

		public List<Movie> GetMoviesFile()
		{
			List<Movie> movieList = new List<Movie>();

			try
			{
				using (var reader = new StreamReader($"{Directory.GetCurrentDirectory()}/File/csv/movielist.csv"))
				using (var csv = new CsvReader(reader, CsvConfiguration))
				{
					csv.Read();
					csv.ReadHeader();
					movieList = csv.GetRecords<Movie>().ToList();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error GetMoviesFile {ex.Message}");
			}

			return movieList;
		}

		public void SaveDbMovies(List<Movie> movieList)
		{
			_movieRepository.AddBulkInsert(movieList);
		}
	}
}
