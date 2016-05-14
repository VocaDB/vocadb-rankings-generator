using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using VocaDBRankings.DataContracts;
using VocaDBRankings.Resources;
using VocaDBRankings.ViewModels;

namespace VocaDBRankings {

	class Program {

		private static DateTime GetFirstDayOfWeek(DateTime dateTime) {

			var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
			while (dateTime.DayOfWeek != firstDayOfWeek) {
				dateTime = dateTime.AddDays(-1);
			}

			return dateTime;

		}

		private static SongForApiContract[] GetSongs(DateTime dateTime) {

			Console.WriteLine("Getting rankings from VocaDB.");

			using (var client = new HttpClient()) {

				client.BaseAddress = new Uri("http://vocadb.net/");
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				
				var clientTask = client.GetAsync("api/songs/top-rated?filterBy=PublishDate&durationHours=168&languagePreference=English&fields=AdditionalNames,ThumbUrl,Tags,PVs&startDate=" + dateTime.ToString("o"));
				clientTask.Wait();
				var response = clientTask.Result;
				response.EnsureSuccessStatusCode();

				var responseTask = response.Content.ReadAsAsync<SongForApiContract[]>();
				responseTask.Wait();
				return responseTask.Result;

			}

		}

		private static string ReadTemplate() {

			if (File.Exists("Template.cshtml"))
				return File.ReadAllText("Template.cshtml");
			else
				return ResourceHelper.ReadTextFile("Template.cshtml");

		}

		static void Main(string[] args) {

			var dateTime = args.Length > 1 ? DateTime.Parse(args[1]) : GetFirstDayOfWeek(DateTime.Now.AddDays(-6)).Date;
			var songs = GetSongs(dateTime);

			var topSongs = songs.Take(3);
			var otherSongs = songs.Skip(3);
			var weekNum = GetIso8601WeekOfYear(dateTime);

			Console.WriteLine("Generating document for date " + dateTime.ToShortDateString() + " (week " + weekNum + ").");

			var viewModel = new TemplateViewModel { TopRatedSongs = topSongs.ToArray(), OtherSongs = otherSongs.ToArray(), WeekNumber = weekNum };

			var config = new TemplateServiceConfiguration();
			config.CachingProvider = new DefaultCachingProvider(t => { });
			var service = RazorEngineService.Create(config);
			Engine.Razor = service;
			var template = ReadTemplate();
			string html;
			try {
				html = Engine.Razor.RunCompile(template, "rankingsTemplate", typeof(TemplateViewModel), viewModel);
			} catch (TemplateCompilationException x) {
				Console.WriteLine("Unable to compile Razor template: " + x.Message);
				Console.ReadLine();
				return;
			}

			var folder = args.FirstOrDefault() ?? string.Empty;
			var baseFileName = Path.Combine(folder, dateTime.Year + "-" + weekNum);
			var file = baseFileName + ".html";

			Console.WriteLine("Writing to " + file);

			File.WriteAllText(file, html, System.Text.Encoding.UTF8);

			var jsonFile = baseFileName + ".json";
			var json = JsonConvert.SerializeObject(songs, Formatting.Indented);
			File.WriteAllText(jsonFile, json, System.Text.Encoding.UTF8);

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
