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
			var LIKE = ToTerm( "LIKE" );
			var NULL = ToTerm( "NULL" );
			var DISTINCT = ToTerm( "DISTINCT" );
			var EQ = ToTerm( "=" );
			var LT = ToTerm( "<" );
			var GT = ToTerm( ">" );
			var LTE = ToTerm( "<=" );
			var GTE = ToTerm( ">=" );
			var NEQ1 = ToTerm( "<>" );
			var NEQ2 = ToTerm( "!=" );
			var ALL = ToTerm( "ALL" );
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
			var IN = ToTerm( "IN" );
			var BETWEEN = ToTerm( "BETWEEN" );
			var EXISTS = ToTerm( "EXISTS" );
			var COUNT = ToTerm( "COUNT" );
			var MAX = ToTerm( "MAX" );
			var MIN = ToTerm( "MIN" );
			var AVG = ToTerm( "AVG" );
			var SUM = ToTerm( "SUM" );
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
			var subexpression = new NonTerminal( "subexpression" );
			var exprList = new NonTerminal( "exprList" );
			var filterOpt = new NonTerminal( "filterOpt" );
			var selectExprList = new NonTerminal( "selectExprList" );
			var from = new NonTerminal( "from" );
			var groupByOpt = new NonTerminal( "groupByOpt" );
			var havingOpt = new NonTerminal( "havingOpt" );
			var orderByOpt = new NonTerminal( "orderByOpt" );
			var limitOpt = new NonTerminal( "limitOpt" );
			var asOpt = new NonTerminal( "asOpt" );
			var aliasOpt = new NonTerminal( "aliasOpt" );
			var tuple = new NonTerminal( "tuple" );
			var joinChainOpt = new NonTerminal( "joinChainOpt" );
			var join = new NonTerminal( "join" );
			var joinKindOpt = new NonTerminal( "joinKindOpt" );
			var condition = new NonTerminal( "condtion" );
			var conditionRhsOpt = new NonTerminal( "conditionRhsOpt" );
			var unOp = new NonTerminal( "unOp" );
			var binOp = new NonTerminal( "binOp" );
			var notOpt = new NonTerminal( "notOpt" );
			var stmtLine = new NonTerminal( "stmtLine" );
			var semiOpt = new NonTerminal( "semiOpt" );
			var stmtList = new NonTerminal( "stmtList" );
			var tableSpec = new NonTerminal( "tableSpec" );
			var aliasedTableRef = new NonTerminal( "aliasedTableRef" );
			var nullsDir = new NonTerminal( "nullsDir" );
			var nullsOpt = new NonTerminal( "nullsOpt" );
			var asKeywordOpt = new NonTerminal( "asKeywordOpt" );
			var joinDir = new NonTerminal( "joinDir" );
			var outerOpt = new NonTerminal( "outerOpt" );
			var selectExpr = new NonTerminal( "selectExpr" );

			var andCondition = new NonTerminal( "andCondition" );
			var booleanCondition = new NonTerminal( "booleanCondition" );
			var operand = new NonTerminal( "operand" );
			var comparisonOperator = new NonTerminal( "opLevel8" );
			var value = new NonTerminal( "value" );
			var column = new NonTerminal( "column" );
			var inClause = new NonTerminal( "inClause" );
			var inItems = new NonTerminal( "inItems" );
			var betweenClause = new NonTerminal( "betweenClause" );
			var existsClause = new NonTerminal( "existsClause" );
			var function = new NonTerminal( "function" );
		
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
			expression.Rule = "(" + subexpression + ")" | subexpression;
			subexpression.Rule = MakePlusRule( subexpression, OR, andCondition );
			andCondition.Rule = MakePlusRule( andCondition, AND, condition );
			booleanCondition.Rule = notOpt + condition;
			notOpt.Rule = Empty | NOT;
			condition.Rule = operand + conditionRhsOpt | existsClause;
			conditionRhsOpt.Rule = Empty | comparisonOperator + operand | IS + ( Empty | NOT ) + NULL | inClause | betweenClause; // TODO: rhs operand not operand
			operand.Rule = value | column | function;
			value.Rule = string_literal | number;
			column.Rule = Id;

			comparisonOperator.Rule = EQ | LT | GT | LTE | GTE | NEQ1 | NEQ2 | LIKE;

			inClause.Rule = notOpt + IN + "(" + ( select | inItems ) + ")";
			inItems.Rule = MakePlusRule( inItems, comma, value );

			betweenClause.Rule = notOpt + BETWEEN + operand + AND + operand;

			existsClause.Rule = notOpt + EXISTS + "(" + select + ")";

			// functions
			function.Rule = ( COUNT | MAX | MIN | AVG | SUM ) + "(" + ( operand | "*" ) + ")";

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