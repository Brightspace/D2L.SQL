using System;
using D2L.ApachePhoenixSQL.Tokenizer;

namespace TestConsoleRunner {
	class Program {
		static void Main( string[] args ) {
			ITokenizer tokenizer = new Tokenizer();

			var tokens = tokenizer.Tokenize( "foo 123     + , abc1 1abc  " );

			foreach( var token in tokens ) {
				Console.WriteLine( token );
			}

			Console.ReadKey();
		}
	}
}
