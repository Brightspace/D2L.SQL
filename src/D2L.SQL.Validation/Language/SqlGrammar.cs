using System;
using Irony.Parsing;

namespace D2L.SQL.Language {
	[Language( "D2L-SQL", "0.1", "D2L-flavored Read-Only SQL" )]
	internal sealed class SqlGrammar : Grammar {

		public SqlGrammar()
			: base( caseSensitive: false ) {

			#region Terminals
			var comment = new CommentTerminal( "comment", "/*", "*/" );
			comment.Priority = TerminalPriority.High;
			var lineComment = new CommentTerminal( "line_comment", "--", "\n", "\r\n" );
			var slashComment = new CommentTerminal( "slash_comment", "//", "\n", "\r\n" );
			NonGrammarTerminals.Add( comment );
			NonGrammarTerminals.Add( lineComment );
			NonGrammarTerminals.Add( slashComment );
			var number = new NumberLiteral( "number" );
			number.DefaultIntTypes = new TypeCode[] { TypeCode.Int64 };
			var string_literal = new StringLiteral( "string", "'", StringOptions.AllowsDoubledQuote );
			var Id_simple = TerminalFactory.CreateSqlExtIdentifier( this, "id_simple" );

			var comma = ToTerm( "," );
			var dot = ToTerm( "." );
			var NOT = ToTerm( "NOT" );
			var ON = ToTerm( "ON" );
			var JOIN = ToTerm( "JOIN" );
			var FIRST = ToTerm( "FIRST" );
			var BY = ToTerm( "BY" );
			var LEFT = ToTerm( "LEFT" );
			var UNION = ToTerm( "UNION" );
			var OR = ToTerm( "OR" );
			var AND = ToTerm( "AND" );
			var IS = ToTerm( "IS" );
			var ANY = ToTerm( "ANY" );
			var LIKE = ToTerm( "LIKE" );
			var NULL = ToTerm( "NULL" );
			var AVG = ToTerm( "AVG" );
			var COUNT = ToTerm( "COUNT" );
			var DISTINCT = ToTerm( "DISTINCT" );
			var CONCAT = ToTerm( "||" );
			var TRUE = ToTerm( "TRUE" );
			var FALSE = ToTerm( "FALSE" );
			var PLUS = ToTerm( "+" );
			var MINUS = ToTerm( "-" );
			var MULT = ToTerm( "*" );
			var DIV = ToTerm( "/" );
			var MOD = ToTerm( "%" );
			var EQ = ToTerm( "=" );
			var LT = ToTerm( "<" );
			var GT = ToTerm( ">" );
			var LTE = ToTerm( "<=" );
			var GTE = ToTerm( ">=" );
			var NEQ1 = ToTerm( "<>" );
			var NEQ2 = ToTerm( "!=" );
			var ALL = ToTerm( "ALL" );
			var STAR = ToTerm( "*" );
			var SELECT = ToTerm( "SELECT" );
			var WHERE = ToTerm( "WHERE" );
			var GROUP = ToTerm( "GROUP" );
			var HAVING = ToTerm( "HAVING" );
			var LIMIT = ToTerm( "LIMIT" );
			var FROM = ToTerm( "FROM" );
			var INNER = ToTerm( "INNER" );
			var OUTER = ToTerm( "OUTER" );
			var RIGHT = ToTerm( "RIGHT" );
			var ORDER = ToTerm( "ORDER" );
			var ASC = ToTerm( "ASC" );
			var DESC = ToTerm( "DESC" );
			var NULLS = ToTerm( "NULLS" );
			var LAST = ToTerm( "LAST" );
			#endregion

			#region Non-terminals
			var Id = new NonTerminal( "Id" );
			var stmt = new NonTerminal( "stmt" );
			var select = new NonTerminal( "select" );
			var selectStmt = new NonTerminal( "selectStmt" );
			var union = new NonTerminal( "union" );
			var unionList = new NonTerminal( "unionList" );
			var idlist = new NonTerminal( "idlist" );
			var orderList = new NonTerminal( "orderList" );
			var order = new NonTerminal( "order" );
			var orderDirOpt = new NonTerminal( "orderDirOpt" );
			var whereOpt = new NonTerminal( "whereOpt" );
			var expression = new NonTerminal( "expression" );
			var exprList = new NonTerminal( "exprList" );
			var filterOpt = new NonTerminal( "filterOpt" );
			var selectExprList = new NonTerminal( "selectExprList" );
			var from = new NonTerminal( "from" );
			var groupByOpt = new NonTerminal( "groupByOpt" );
			var havingOpt = new NonTerminal( "havingOpt" );
			var orderByOpt = new NonTerminal( "orderByOpt" );
			var limitOpt = new NonTerminal( "limitOpt" );
			var columnItemList = new NonTerminal( "columnItemList" );
			var columnItem = new NonTerminal( "columnItem" );
			var asOpt = new NonTerminal( "asOpt" );
			var aliasOpt = new NonTerminal( "aliasOpt" );
			var tuple = new NonTerminal( "tuple" );
			var joinChainOpt = new NonTerminal( "joinChainOpt" );
			var join = new NonTerminal( "join" );
			var joinKindOpt = new NonTerminal( "joinKindOpt" );
			var condition = new NonTerminal( "condtion" );
			var conditionRhsOpt = new NonTerminal( "conditionRhsOpt" );
			var term = new NonTerminal( "term" );
			var unExpr = new NonTerminal( "unExpr" );
			var unOp = new NonTerminal( "unOp" );
			var binExpr = new NonTerminal( "binExpr" );
			var binOp = new NonTerminal( "binOp" );
			var betweenExpr = new NonTerminal( "betweenExpr" );
			var parSelectStmt = new NonTerminal( "parSelectStmt" );
			var notOpt = new NonTerminal( "notOpt" );
			var funCall = new NonTerminal( "funCall" );
			var stmtLine = new NonTerminal( "stmtLine" );
			var semiOpt = new NonTerminal( "semiOpt" );
			var stmtList = new NonTerminal( "stmtList" );
			var funArgs = new NonTerminal( "funArgs" );
			var inStmt = new NonTerminal( "inStmt" );
			var tableSpec = new NonTerminal( "tableSpec" );
			var aliasedTableRef = new NonTerminal( "aliasedTableRef" );
			var nullsDir = new NonTerminal( "nullsDir" );
			var nullsOpt = new NonTerminal( "nullsOpt" );
			var asKeywordOpt = new NonTerminal( "asKeywordOpt" );
			var allFamily = new NonTerminal( "allFamily" );
			var joinDir = new NonTerminal( "joinDir" );
			var outerOpt = new NonTerminal( "outerOpt" );
			var expression2 = new NonTerminal( "expression2" );
			var expression3 = new NonTerminal( "expression3" );
			var selectExpr = new NonTerminal( "selectExpr" );

			var andCondition = new NonTerminal( "andCondition" );
			var booleanCondition = new NonTerminal( "booleanCondition" );
			var booleanTest = new NonTerminal( "booleanTest" );
			var operand = new NonTerminal( "operand" );
			var rhsOperand = new NonTerminal( "rhsOperand" );
			var anyOrAll = new NonTerminal( "anyOrAll" );
			var like = new NonTerminal( "like" );
			var summand = new NonTerminal( "summand" );
			var factor = new NonTerminal( "factor" );
			var opLevel8 = new NonTerminal( "opLevel8" );
			var opLevel9 = new NonTerminal( "opLevel9" );
			var opLevel10 = new NonTerminal( "opLevel10" );
			var indexOpt = new NonTerminal( "indexOpt" );
			var term2 = new NonTerminal( "term2" );
			var value = new NonTerminal( "value" );
			var function = new NonTerminal( "function" );
			var rhsOpt = new NonTerminal( "rhsOpt" );
			var column = new NonTerminal( "column" );
			var truthValue = new NonTerminal( "truthValue" );
			var booleanPrimary = new NonTerminal( "booleanPrimary" );
			var predicate = new NonTerminal( "predicate" ); // continue from here with BNF
			#endregion

			#region Base grammar
			Root = stmtList;
			stmtList.Rule = MakePlusRule( stmtList, stmtLine );
			stmtLine.Rule = stmt + semiOpt;
			stmt.Rule = select;
			semiOpt.Rule = Empty | ";";
			#endregion

			#region SELECT grammar
			// SELECT
			select.Rule = selectStmt + unionList + orderByOpt + limitOpt;
			selectStmt.Rule = SELECT + filterOpt + selectExprList + from + joinChainOpt + whereOpt + groupByOpt + havingOpt;
			filterOpt.Rule = Empty | ALL | DISTINCT;
			whereOpt.Rule = Empty | WHERE + expression;
			havingOpt.Rule = Empty | HAVING + expression;
			groupByOpt.Rule = Empty | GROUP + BY + exprList;
			limitOpt.Rule = Empty | LIMIT + number;

			// Column list
			selectExprList.Rule = MakePlusRule( selectExprList, comma, selectExpr );
			selectExpr.Rule = "*" | Id_simple + ".*" | expression + asOpt;

			// FROM
			from.Rule = FROM + tableSpec;
			tableSpec.Rule = aliasedTableRef | "(" + select + ")" + asOpt;
			aliasedTableRef.Rule = Id_simple + asOpt;

			// JOIN
			joinChainOpt.Rule = MakeStarRule( joinChainOpt, join );
			join.Rule = joinKindOpt + JOIN + tableSpec + ON + expression;
			joinKindOpt.Rule = Empty | INNER | joinDir + outerOpt;
			joinDir.Rule = LEFT | RIGHT;
			outerOpt.Rule = Empty | OUTER;

			// UNION ALL
			unionList.Rule = MakeStarRule( unionList, union );
			union.Rule = UNION + ALL + selectStmt;

			// ORDER BY
			orderByOpt.Rule = Empty | ORDER + BY + orderList;
			orderList.Rule = MakePlusRule( orderList, comma, order );
			order.Rule = Id + orderDirOpt + nullsOpt;
			orderDirOpt.Rule = Empty | ASC | DESC;
			nullsOpt.Rule = Empty | NULLS + nullsDir;
			nullsDir.Rule = FIRST | LAST;

			// AS
			asOpt.Rule = Empty | asKeywordOpt + Id_simple;
			asKeywordOpt.Rule = Empty | "AS";
			#endregion

			#region Expression grammar
			exprList.Rule = MakePlusRule( exprList, comma, expression );
			expression.Rule = MakePlusRule( expression, OR, andCondition );
			andCondition.Rule = MakePlusRule( andCondition, AND, condition );
			booleanCondition.Rule = notOpt + condition;
			notOpt.Rule = Empty | NOT;
			condition.Rule = operand + conditionRhsOpt;
			conditionRhsOpt.Rule = Empty | opLevel8 + operand | IS + ( Empty | NOT ) + NULL; // TODO: rhs operand not operand
			operand.Rule = value | column;
			value.Rule = string_literal | number;
			column.Rule = Id;

			opLevel8.Rule = EQ | LT | GT | LTE | GTE | NEQ1 | NEQ2 | LIKE;

			// Id
			Id.Rule = Id_simple + ( Empty | dot + Id_simple );
			idlist.Rule = MakePlusRule( idlist, comma, Id );

			#endregion

			MarkPunctuation( ",", "(", ")" );
			MarkPunctuation( semiOpt, asKeywordOpt );
			MarkTransient( stmt, aliasOpt, asKeywordOpt, stmtLine, unOp, tuple );
			binOp.SetFlag( TermFlags.InheritPrecedence );
		}
	}
}