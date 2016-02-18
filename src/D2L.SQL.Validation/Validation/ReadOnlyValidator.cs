using D2L.SQL.Language;
using Irony.Parsing;

namespace D2L.SQL.Validation {
	/// <summary>
	/// A configurable validator for read-only (i.e. SELECTs) SQL
	/// </summary>
	public sealed class ReadOnlyValidator : IValidator {
		private readonly ITablePolicy m_tablePolicy;
		private readonly Parser m_parser;

		private sealed class DefaultTablePolicy : ITablePolicy {
			public bool CheckIfTableIsAllowed( string schema, string tableName ) {
				return true;
			}
		}

		/// <summary>
		/// Construct a ReadOnlyValidator 
		/// </summary>
		/// <param name="tablePolicy">A table policy</param>
		public ReadOnlyValidator(
			ITablePolicy tablePolicy = null
		) {
			m_tablePolicy = tablePolicy ?? new DefaultTablePolicy();
			m_parser = new Parser( new SqlGrammar() );
		}

		/// <inheritdoc/>
		string IValidator.Sanitize( string sql ) {
			ParseTree parseTree = m_parser.Parse( sql );
			
			if( parseTree.Root == null ) {
				throw new SqlValidationException();
			}

			return sql;
		}

		/// <summary>
		/// The tables to allow reads from (FROM, JOIN etc.)
		/// </summary>
		public ITablePolicy TablePolicy {
			get { return m_tablePolicy; }
		}
	}
}