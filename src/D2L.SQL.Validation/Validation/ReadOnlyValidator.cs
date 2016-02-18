using D2L.SQL.Language;

namespace D2L.SQL.Validation {
	/// <summary>
	/// A configurable validator for read-only (i.e. SELECTs) SQL
	/// </summary>
	public sealed class ReadOnlyValidator : IValidator {
		private readonly SqlGrammar m_grammar;

		/// <summary>
		/// Construct a ReadOnlyValidator 
		/// </summary>
		/// <param name="tablePolicy">A table policy</param>
		public ReadOnlyValidator(
			ITablePolicy tablePolicy = null
		) {
			m_grammar = new SqlGrammar( tablePolicy );
		}

		/// <inheritdoc/>
		string IValidator.Sanitize( string sql ) {
			m_grammar.Parse( sql );

			return sql;
		}

		/// <summary>
		/// The tables to allow reads from (FROM, JOIN etc.)
		/// </summary>
		public ITablePolicy TablePolicy {
			get { return m_grammar.TablePolicy; }
		}
	}

}