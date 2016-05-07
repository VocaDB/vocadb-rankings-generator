using System.IO;

namespace VocaDBRankings.Resources {

	public class ResourceHelper {

		public static string ReadTextFile(string fileName) {

			var asm = typeof(ResourceHelper).Assembly;
			using (var stream = asm.GetManifestResourceStream(asm.GetName().Name + ".Resources." + fileName))
			using (var reader = new StreamReader(stream)) {

				return reader.ReadToEnd();

			}

		}

	}
}
