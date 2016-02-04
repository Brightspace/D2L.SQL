using System.Collections.Generic;

namespace D2L.ApachePhoenixSQL.Tokenizer {
	public interface ITokenizer {
		IEnumerable<Token> Tokenize( string s );
	}
}
