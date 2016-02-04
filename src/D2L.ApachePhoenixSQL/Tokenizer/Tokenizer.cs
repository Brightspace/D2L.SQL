using System;
using System.Collections.Generic;

namespace D2L.ApachePhoenixSQL.Tokenizer {
	public class Tokenizer : ITokenizer {
		IEnumerable<Token> ITokenizer.Tokenize( string s ) {
			if ( s == "" ) {
				yield break;
			}

			var ctx = new TokenizerContext {
				State = TokenizerState.BEGIN
			};

			bool done = false;

			do {
				var prevCtx = ctx;
				ctx = Step( prevCtx, s );

				done = ctx.Index == s.Length;

				ctx.Index++;

				if( ctx.NewToken || ( done && ctx.State != TokenizerState.BEGIN ) ) {
					string val = s.Substring(
						startIndex: prevCtx.StartIndex,
						length: prevCtx.Index - prevCtx.StartIndex
					);

					yield return new Token(
						type: StateToTokenType( prevCtx.State ),
						val: val
						);
				}
	 		} while( !done ) ;
		}

		private static TokenizerContext Step(
			TokenizerContext ctx,
			string s
		) {
			ctx.NewToken = false;

			if (ctx.Index == s.Length) {
				if (ctx.State != TokenizerState.BEGIN) {
					ctx.Emit();
				}
				return ctx;
			}

			char c = s[ctx.Index];

			switch(  ctx.State ) {
				case TokenizerState.BEGIN:
					if (IsIdentChar( c ) ) {
						ctx.State = TokenizerState.IDENTIFIER;
					} else if (Char.IsWhiteSpace( c ) ) {
						// Ignore whitespace
					} else if (Char.IsDigit( c ) ) {
						ctx.State = TokenizerState.NUMBER;
					} else {
						switch( c ) {
							case '*': ctx.State = TokenizerState.STAR; break;
							case ',': ctx.State = TokenizerState.COMMA; break;
							case '.': ctx.State = TokenizerState.DOT; break;
							case '+': ctx.State = TokenizerState.PLUS; break;
							default:
								throw new NotImplementedException();
						}
					}
					ctx.StartIndex = ctx.Index;
					return ctx;

				case TokenizerState.IDENTIFIER:
					if (IsIdentChar( c ) || Char.IsDigit( c ) ) {
						return ctx;	
					}
					ctx.Emit();
					return ctx;

				case TokenizerState.NUMBER:
					if ( Char.IsDigit( c ) ) {
						return ctx;
					}
					ctx.Emit();
					return ctx;

				case TokenizerState.STAR:
				case TokenizerState.COMMA:
				case TokenizerState.DOT:
				case TokenizerState.PLUS:
					ctx.Emit();
					return ctx;

				default:
					throw new NotImplementedException();
			}
		}

		private static bool IsIdentChar( char c ) {
			return c == '_' || Char.IsLetter( c );
		}

		private static TokenType StateToTokenType( TokenizerState state ) {
			switch( state ) {
				case TokenizerState.NUMBER: return TokenType.NUMBER;
				case TokenizerState.COMMA: return TokenType.COMMA;
				case TokenizerState.STAR: return TokenType.STAR;
				case TokenizerState.IDENTIFIER: return TokenType.IDENTIFIER;
				case TokenizerState.DOT: return TokenType.DOT;
				case TokenizerState.PLUS: return TokenType.PLUS;
				default:
					throw new NotImplementedException();
			}
		}
	}
}
