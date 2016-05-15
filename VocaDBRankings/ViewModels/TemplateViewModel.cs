using System;
using System.Collections.Generic;
using System.Linq;
using VocaDBRankings.DataContracts;

namespace VocaDBRankings.ViewModels {

	public class TemplateViewModel {

		private PVContract GetMainPV(PVContract[] pvs, PVService service) {

			pvs = pvs.Where(p => p.Service == service).ToArray();

			return pvs.FirstOrDefault(p => p.PVType == PVType.Original) ?? pvs.FirstOrDefault(p => p.PVType == PVType.Reprint) ?? pvs.FirstOrDefault(p => p.PVType == PVType.Other);

		}

		public PVContract[] GetMainPVs(SongForApiContract song) {

			return Enum.GetValues(typeof(PVServices))
				.Cast<PVServices>()
				.Where(service => (song.PVServices & service) == service)
				.Select(service => GetMainPV(song.PVs, (PVService)service))
				.Where(pv => pv != null)
				.ToArray();

		}

		public string GetServiceIcon(PVService service) {

			switch (service) {
				case PVService.Bilibili:
					return "bilibili.png";
				case PVService.NicoNicoDouga:
					return "nico.png";
				case PVService.Piapro:
					return "piapro.png";
				case PVService.SoundCloud:
					return "soundcloud.png";
				case PVService.Youtube:
					return "youtube.png";
				case PVService.Vimeo:
					return "vimeo.png";
			}

			return string.Empty;

		}

		public DateTime BeginDate { get; set; }

		public DateTime EndDate { get; set; }

		public IEnumerable<SongForApiContract> OtherSongs => Songs.Skip(3);

		public SongForApiContract[] Songs { get; set; }

		public IEnumerable<SongForApiContract> TopRatedSongs => Songs.Take(3);

		public int WeekNumber { get; set; }

		public int GetSongPosition(SongForApiContract song) {
			return Array.IndexOf(Songs, song) + 1;
		}

	}

}
