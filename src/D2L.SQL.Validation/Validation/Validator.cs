using System;
using System.Collections.Generic;

namespace D2L.SQL.Validation {
	/// <summary>
	/// A configurable validator for read-only (i.e. SELECTs) SQL
	/// </summary>
	public sealed class ReadOnlyValidator : IValidator {
		private readonly HashSet<string> m_allowedFunctions = new HashSet<string>();
		private readonly ITablePolicy m_tablePolicy;

		/// <summary>
		/// Construct a ReadOnlyValidator 
		/// </summary>
		/// <param name="tablePolicy">A table policy</param>
		public ReadOnlyValidator(
			ITablePolicy tablePolicy
		) {
			m_tablePolicy = tablePolicy;
		}

		/// <inheritdoc/>
		string IValidator.Validate( string sql ) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// The functions which are allowed in the expression sub-grammar (e.g. COUNT, AVG)
		/// The names are case-insensitive (TODO: make sure this is true or rewrite docs)
		/// </summary>
		public HashSet<string> AllowedFunctions {
			get { return m_allowedFunctions; }
		}

		/// <summary>
		/// The tables to allow reads from (FROM, JOIN etc.)
		/// </summary>
		public ITablePolicy TablePolicy {
			get { return m_tablePolicy; }
		}
	}
}