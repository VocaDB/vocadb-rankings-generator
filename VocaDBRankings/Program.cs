using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using VocaDBRankings.DataContracts;
using VocaDBRankings.Resources;

namespace VocaDBRankings {

	class Program {

		static void Main(string[] args) {

			Console.WriteLine("Getting rankings from VocaDB.");
			RunAsync().Wait();

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

		private static async Task RunAsync() {

			using (var client = new HttpClient()) {

				client.BaseAddress = new Uri("http://vocadb.net/");
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var response = await client.GetAsync("api/songs/top-rated");
				response.EnsureSuccessStatusCode();

				var songs = await response.Content.ReadAsAsync<SongForApiContract[]>();

				var topSongs = songs.Take(3);
				var otherSongs = songs.Skip(3);

				Console.WriteLine("Generating document.");

				var template = ResourceHelper.ReadTextFile("Template.html");
				var doc = new HtmlDocument();
				doc.LoadHtml(template);

				var weekNum = doc.GetElementbyId("weekNumber");
				weekNum.InnerHtml = GetIso8601WeekOfYear(DateTime.Now).ToString();

				var generatedAt = doc.GetElementbyId("generatedAt");
				generatedAt.InnerHtml = DateTime.Now.ToString();

				var topRatedSongsTable = doc.GetElementbyId("topRatedSongs");

				foreach (var song in topSongs) {

					var div = topRatedSongsTable.AppendChild(doc.CreateElement("div"));
					div.Attributes.Add("class", "span-4");


				}

				var otherSongsTable = doc.GetElementbyId("songsTable");


			}


		}

	}

}
