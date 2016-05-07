using System.Runtime.Serialization;

namespace VocaDBRankings.DataContracts {

	[DataContract(Namespace = Schemas.VocaDb)]
	public class TagUsageForApiContract {

		[DataMember]
		public int Count { get; set; }

		[DataMember]
		public TagBaseContract Tag { get; set; }

	}

}
