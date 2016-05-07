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

		public SongForApiContract[] OtherSongs { get; set; }

		public SongForApiContract[] TopRatedSongs { get; set; }

		public int WeekNumber { get; set; }

	}

}
