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

			do {
				var prevCtx = ctx;
				ctx = Step( ctx, s );

				if( ctx.NewToken ) {
					string val = s.Substring(
						startIndex: prevCtx.StartIndex - 1,
						length: ctx.StartIndex - prevCtx.StartIndex
					);

					yield return new Token(
						type: StateToTokenType( prevCtx.State ),
						val: val
					);
				}
			} while( ctx.Index != s.Length );
		}

		private static TokenizerContext Step(
			TokenizerContext ctx,
			string s
		) {
			ctx.NewToken = false;

			char c = s[ctx.Index];

			ctx.Index++;

			switch(  ctx.State ) {
				case TokenizerState.BEGIN:
					if (IsIdentChar( c ) ) {
						ctx.State = TokenizerState.IDENTIFIER;
					} else if (Char.IsWhiteSpace( c ) ) {
						return ctx;
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
					if (IsIdentChar( c )  || Char.IsDigit( c ) ) {
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
