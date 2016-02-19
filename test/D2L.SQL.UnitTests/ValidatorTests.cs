using D2L.SQL.Validation;
using NUnit.Framework;

namespace D2L.SQL.UnitTests {

	[TestFixture]
	internal sealed class ValidatorTests {

		private IValidator m_validator;

		[SetUp]
		public void BeforeAll() {
			m_validator = new ReadOnlyValidator();
		}

		[TestCase( "SELECT * FROM atable" )]
		[TestCase( "SELECT columnA, column2, THREE FROM atable" )]
		[TestCase( "SELECT info as theInfo, other FROM atable" )]
		[TestCase( @"SELECT info as theInfo, 
					-- line comment
						other
					FROM atable" )]
		[TestCase( "SELECT * FROM atable // a comment" )]
		[TestCase( @"SELECT * FROM /* a
						multiline
						comment */ atable" )]
		[TestCase( "SELECT * FROM table WHERE col IS NULL" )]
		[TestCase( "SELECT * FROM table WHERE col IS NOT NULL" )]
		[TestCase( "SELECT * FROM table JOIN (SELECT * FROM othertable) ON table.a = othertable.b" )]
		[TestCase( "SELECT * FROM table JOIN (SELECT * FROM othertable)/* internal comment */ ON table.a = othertable.b" )]
		[TestCase( "SELECT * FROM table JOIN (SELECT * FROM othertable) ON table.a = othertable.b LEFT OUTER JOIN (SELECT a,b FROM THIRDTABLE WHERE a = 5) ON table.c = THIRDTABLE.x;" )]
		[TestCase( @"/* comment here */ SELECT ORGUNITID AS COURSE, ROLEID AS ROLE, TOOLID AS TOOL, TIMESTAMP, V AS NUM_ACCESSES FROM TOOL_ACCESS_BY_COURSE_ROLE_TOOL;" )]
		[TestCase( "SELECT TV1.TAGV AS ROLE, TIMESTAMP, V AS LOGINCOUNT FROM VBSPACE_EVENTS_AGGR_218 AS LC WHERE TIMESTAMP>=142007044000;" )]
		[TestCase( "SELECT ROLEID AS ROLE, TIMESTAMP, V AS LOGINCOUNT FROM VBSPACE_EVENTS_AGGR_218 WHERE TIMESTAMP>=142007044000;" )]
		[TestCase( "SELECT TV1.TAGV AS ROLE, TIMESTAMP, V AS LOGINCOUNT FROM VBSPACE_EVENTS_AGGR_218 AS LC LEFT OUTER JOIN VBSPACE_UIDS UID " +
				   "ON LC.ROLEID = UID.UID WHERE TIMESTAMP>=142007044000;" )]
		[TestCase( "SELECT TV1.TAGV AS ROLE, TIMESTAMP, V AS LOGINCOUNT FROM VBSPACE_EVENTS_AGGR_218 AS LC " +
				   "LEFT OUTER JOIN (SELECT UID, TAGV FROM VBSPACE_UIDS WHERE TAGV IS NOT NULL) TV1 ON LC.ROLEID = TV1.UID WHERE TIMESTAMP>=142007044000;" )]
		[TestCase(@"/* multiline query */

					SELECT

					TV1.TAGV AS COURSE,

					TV2.TAGV AS ROLE,

					TV3.TAGV AS TOOL,

					TIMESTAMP,

					V AS NUM_ACCESSES -- ignore

					FROM TOOL_ACCESS_BY_COURSE_ROLE_TOOL AS TA

					LEFT OUTER JOIN (SELECT UID, TAGV FROM VBSPACE_UIDS WHERE TAGV IS NOT NULL) TV1 ON TA.ORGUNITID = TV1.UID

					LEFT OUTER JOIN (SELECT UID, TAGV FROM VBSPACE_UIDS WHERE TAGV IS NOT NULL) TV2 ON TA.ROLEID= TV2.UID

					LEFT OUTER JOIN (SELECT UID, TAGV FROM VBSPACE_UIDS WHERE TAGV IS NOT NULL) TV3 ON TA.TOOLID= TV3.UID;")]
		[TestCase( "SELECT info as theInfo, other FROM atable GROUP BY x HAVING y = 4 AND g IS NOT NULL ORDER BY time" )]
		[TestCase( "SELECT column from A WHERE column like 'my data'" )]
		[TestCase( "SELECT id, colour from CATS WHERE A IN (1, 2, 3)" )]
		[TestCase( "select * from target where id in (SELECT id FROM source WHERE COST > 10)")]
		public void SanitizeReturnsInput_GivenValidSql( string sql ) {
			// The interface doesn't actually guarantee that the output equals the input, but this is the easiest test given the current implementation
			Assert.That( m_validator.Sanitize( sql ), Is.EqualTo( sql ) );
		}

		[TestCase( "" )]
		[TestCase( "DELETE FROM TABLE" )]
		[TestCase( "UPDATE STATISTICS table" )]
		[TestCase( "ALTER INDEX a ON sometable REBUILD" )]
		[TestCase( "DROP INDEX a ON sometable REBUILD" )]
		[TestCase( "CREATE SEQUENCE seq" )]
		[TestCase( "DROP VIEW criticalview" )]
		[TestCase( "UPSERT INTO table VALUES (1, 2, 3)" )]
		[TestCase( "select * from target where id in (DELETE FROM source WHERE COST > 10)" )]
		public void SanitizeThrows_GivenInvalidSql( string sql ) {
			Assert.Throws<SqlValidationException>( () => m_validator.Sanitize( sql ) );
		}

		[TestCase( "select * from SYSTEM.table" )]
		[TestCase( "select any, thing as t from anyschema.table" )]
		[TestCase( "select * from table t JOIN system.STATS as sneak ON t.id = sneak.id;" )]
		[TestCase( "select * from (SELECT * FROM System.STATS)" )]
		[TestCase( "SELECT A, B AS C FROM /* despite the comment */ \"SYSTEM\".secrets" )]
		[TestCase( "SELECT A, B AS C FROM SYSTEM/* despite the comment */.secrets" )]
		[TestCase( "SELECT A, B AS C FROM \"/*despite the comment */SYS/*---*/TEM/* despite all comments */\".secrets" )]
		[TestCase( "SELECT A, B AS C FROM \"SYSTEM/* despite the comment */\".secrets" )]
		public void SanitizeThrows_IfASystemTableIsReferenced( string sql ) {
			Assert.Throws<SqlValidationException>( () => m_validator.Sanitize( sql ) );
		}
	}
}
