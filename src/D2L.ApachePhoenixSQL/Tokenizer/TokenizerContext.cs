namespace D2L.ApachePhoenixSQL.Tokenizer {
	internal struct TokenizerContext {
		public TokenizerState State;
		public int Index;
		public int StartIndex;
		public bool NewToken;

		/// <summary>
		/// Call when Index points at the last char of a token
		/// </summary>
		public void Emit() {
			State = TokenizerState.BEGIN;
			Index -= 1;
			StartIndex = Index;
			NewToken = true;
		}
	}
}
