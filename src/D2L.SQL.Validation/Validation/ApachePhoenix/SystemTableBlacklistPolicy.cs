using System.Text.RegularExpressions;

namespace D2L.SQL.Validation.ApachePhoenix {
	/// <summary>
	/// Blacklist SYSTEM.* tables.
	/// </summary>
	public class SystemTableBlacklistPolicy : ITablePolicy {
		bool ITablePolicy.CheckIfTableIsAllowed( string schema, string tableName ) {
			// strip out comments, as Irony won't remove them from inside quotation marks, and it is conceivable that Phoenix could
			// now or in the future strip them out even here
			return schema == null || Regex.Replace( schema.ToLower(), "/\\*.*?\\*/", "", RegexOptions.Compiled ) != "system";
		}
	}
}
