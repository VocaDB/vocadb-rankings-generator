using System.Runtime.Serialization;

namespace VocaDBRankings.DataContracts {

	/// <summary>
	/// Entry thumbnail for API.
	/// Contains URLs to thumbnails of different sizes.
	/// Does not include URL to original picture at the moment because that is loaded differently.
	/// </summary>
	/// <remarks>
	/// Default sizes are described in ImageSize, but the sizes might vary depending on entry type.
	/// For example, song thumbnails have different sizes.
	/// </remarks>
	[DataContract(Namespace = Schemas.VocaDb)]
	public class EntryThumbForApiContract {

		/// <summary>
		/// URL to small thumbnail.
		/// Default size is 150x150px.
		/// </summary>
		[DataMember]
		public string UrlSmallThumb { get; set; }

		/// <summary>
		/// URL to large thumbnail.
		/// Default size is 250x250px.
		/// </summary>
		[DataMember]
		public string UrlThumb { get; set; }

		/// <summary>
		/// URL to tiny thumbnail.
		/// Default size is 70x70px.
		/// </summary>
		[DataMember]
		public string UrlTinyThumb { get; set; }

	}

}
