namespace D2L.SQL.Validation {
	/// <summary>
	/// Policy on access to particular tables
	/// </summary>
	public interface ITablePolicy {
		/// <summary>
		/// Checks if access to a table should be permitted
		/// </summary>
		/// <param name="schema">The name of the table schema</param>
		/// <param name="tableName">The name of the table</param>
		/// <returns>true if access to the table should be permitted</returns>
		bool CheckIfTableIsAllowed( string schema, string tableName );
	}
}