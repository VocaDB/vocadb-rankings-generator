using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VocaDBRankings.DataContracts {

	[DataContract(Namespace = Schemas.VocaDb)]
	public class PVContract {

		[DataMember]
		public string Author { get; set; }

		[DataMember]
		public int Id { get; set; }

		/// <summary>
		/// Length in seconds, 0 if not specified.
		/// </summary>
		[DataMember]
		public int Length { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public DateTime? PublishDate { get; set; }

		[DataMember]
		public string PVId { get; set; }

		[DataMember]
		[JsonConverter(typeof(StringEnumConverter))]
		public PVService Service { get; set; }

		[DataMember]
		[JsonConverter(typeof(StringEnumConverter))]
		public PVType PVType { get; set; }

		[DataMember]
		public string ThumbUrl { get; set; }

		[DataMember]
		public string Url { get; set; }

	}

}
