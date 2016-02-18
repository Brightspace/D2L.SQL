namespace D2L.SQL.Validation {
	internal sealed class DefaultTablePolicy : ITablePolicy {
		public bool CheckIfTableIsAllowed( string schema, string tableName ) {
			return true;
		}
	}
}