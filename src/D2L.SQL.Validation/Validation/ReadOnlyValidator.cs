using D2L.SQL.Language;
using Irony.Parsing;

namespace D2L.SQL.Validation {
	/// <summary>
	/// A configurable validator for read-only (i.e. SELECTs) SQL
	/// </summary>
	public sealed class ReadOnlyValidator : IValidator {

		private readonly Parser m_parser;

		/// <summary>
		/// Construct a ReadOnlyValidator 
		/// </summary>
		public ReadOnlyValidator() {
			m_parser = new Parser( new SqlGrammar() );
		}

		/// <inheritdoc/>
		string IValidator.Sanitize( string sql ) {
			ParseTree parseTree = m_parser.Parse( sql );

			if( parseTree.Root == null ) {
				throw new SqlValidationException( parseTree.ParserMessages );
			}

			return sql;
		}
	}

}