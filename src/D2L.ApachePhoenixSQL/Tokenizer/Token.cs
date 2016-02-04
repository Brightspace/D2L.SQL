namespace D2L.ApachePhoenixSQL.Tokenizer {
	public struct Token {
		private readonly TokenType m_type;
		private readonly string m_value;

		public Token( TokenType type, string val ) {
			m_type = type;
			m_value = val;
		}

		public TokenType Type {
			get { return m_type; }
		}

		public string Value {
			get { return m_value; }
		}

		public override string ToString() {
			return string.Format( "{0}[ {1} ]", m_type, m_value );
		}
	}
}
