namespace D2L.SQL.Validation {
	/// <summary>
	/// Safetly validates raw SQL (e.g. from a user) against a set of policies.
	/// </summary>
	public interface IValidator {
		/// <summary>
		/// Validate possibly-unsafe SQL against a set of policies. 
		/// </summary>
		/// <param name="sql">Raw SQL (e.g. from a user)</param>
		/// <returns>SQL that is guaranteed to pass validation</returns>
		/// <throws>TODO</throws>
		string Sanitize( string sql );
	}
}