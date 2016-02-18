using System;
using System.Collections.Generic;
using System.Linq;
using Irony;

namespace D2L.SQL.Validation {
	/// <summary>
	/// Base class for SQL validation errors
	/// </summary>
	public class SqlValidationException : Exception {

		/// <summary>
		/// Indicates that the sql was not valid.
		/// </summary>
		/// <param name="message">Should indicate what was wrong with the sql.</param>
		public SqlValidationException( string message )
			: base( message ) { }

		/// <summary>
		/// Indicates that the sql was not valid
		/// </summary>
		/// <param name="messages">Messages from Irony parser</param>
		public SqlValidationException( IEnumerable<LogMessage> messages )
			: this( string.Join( ";", messages.Select( m =>
				String.Format( 
					"at {0}, {1}",
					m.Location,
					m.Message
				) 
			) ) ) { }

	}
}