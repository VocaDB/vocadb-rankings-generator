using VocaDBRankings.DataContracts;

namespace VocaDBRankings.ViewModels {

	public class TemplateViewModel {

		public SongForApiContract[] OtherSongs { get; set; }

		public SongForApiContract[] TopRatedSongs { get; set; }

		public int WeekNumber { get; set; }

	}

}
