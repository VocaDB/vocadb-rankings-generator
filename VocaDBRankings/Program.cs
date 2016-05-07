using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using VocaDBRankings.DataContracts;
using VocaDBRankings.Resources;
using VocaDBRankings.ViewModels;

namespace VocaDBRankings {

	class Program {

		static void Main(string[] args) {

			Console.WriteLine("Getting rankings from VocaDB.");

			SongForApiContract[] songs;

			using (var client = new HttpClient()) {

				client.BaseAddress = new Uri("http://vocadb.net/");
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var clientTask = client.GetAsync("api/songs/top-rated?durationHours=168&fields=AdditionalNames,ThumbUrl,Tags,PVs");
				clientTask.Wait();
				var response = clientTask.Result;
				response.EnsureSuccessStatusCode();

				var responseTask = response.Content.ReadAsAsync<SongForApiContract[]>();
				responseTask.Wait();
				songs = responseTask.Result;

			}

			var topSongs = songs.Take(3);
			var otherSongs = songs.Skip(3);
			var weekNum = GetIso8601WeekOfYear(DateTime.Now);

			Console.WriteLine("Generating document.");

			var viewModel = new TemplateViewModel { TopRatedSongs = topSongs.ToArray(), OtherSongs = otherSongs.ToArray(), WeekNumber = weekNum };

			var config = new TemplateServiceConfiguration();
			config.CachingProvider = new DefaultCachingProvider(t => { });
			var service = RazorEngineService.Create(config);
			Engine.Razor = service;
			var template = ResourceHelper.ReadTextFile("Template.cshtml");
			var html = Engine.Razor.RunCompile(template, "rankingsTemplate", typeof(TemplateViewModel), viewModel);

			var folder = args.FirstOrDefault() ?? string.Empty;
			var file = Path.Combine(folder, DateTime.Now.Year + "-" + weekNum + ".html");

			Console.WriteLine("Writing to " + file);

			File.WriteAllText(file, html, System.Text.Encoding.UTF8);

		}

		// This presumes that weeks start with Monday.
		// Week 1 is the 1st week of the year with a Thursday in it.
		// From http://stackoverflow.com/a/11155102
		private static int GetIso8601WeekOfYear(DateTime time) {
			// Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
			// be the same week# as whatever Thursday, Friday or Saturday are,
			// and we always get those right
			DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
			if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday) {
				time = time.AddDays(3);
			}

			// Return the week of our adjusted day
			return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
		}

	}

}
