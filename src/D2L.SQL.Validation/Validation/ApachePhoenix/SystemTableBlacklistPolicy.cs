namespace D2L.SQL.Validation.ApachePhoenix {
	/// <summary>
	/// Blacklist SYSTEM.* tables.
	/// </summary>
	public class SystemTableBlacklistPolicy : ITablePolicy {
		bool ITablePolicy.CheckIfTableIsAllowed( string schema, string tableName ) {
			return schema.ToLower() != "system";
		}
	}
}
