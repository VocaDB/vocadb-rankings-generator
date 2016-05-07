using System.Runtime.Serialization;

namespace VocaDBRankings.DataContracts {

	[DataContract(Namespace = Schemas.VocaDb)]
	public class TagBaseContract {

		/// <summary>
		/// Additional names - optional field.
		/// </summary>
		[DataMember(EmitDefaultValue = false)]
		public string AdditionalNames { get; set; }

		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string UrlSlug { get; set; }

	}

}
