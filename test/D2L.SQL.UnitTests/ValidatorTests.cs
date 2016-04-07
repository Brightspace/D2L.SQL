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

		[TestCase( "SELECT F.10ABC, TAGS[2] FROM SOMEVIEW WHERE TIMESTAMP = 1352419200000 AND WIDGETTYPE = 3" )]
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
		[TestCase( "SELECT * FROM table WHERE col IS NULL UNION ALL SELECT info as theInfo, other FROM atable" )]
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
		[TestCase( "SELECT id, colour from CATS WHERE ID IN (1, 2, 3)" )]
		[TestCase( "SELECT id, colour from CATS WHERE ID NOT IN (1, 2, 3)" )]
		[TestCase( "select * from target where id in (SELECT id FROM source WHERE COST > 10)" )]
		[TestCase( "SELECT id, weight from CATS WHERE weight BETWEEN 10 AND 20;" )]
		[TestCase( "SELECT id, weight from CATS WHERE weight NOT BETWEEN 10 AND 20;" )]
		[TestCase( "SELECT id, colour from CATS WHERE EXISTS (SELECT colour FROM wallpapers)" )]
		[TestCase( "SELECT id, colour from CATS WHERE NOT EXISTS (SELECT colour FROM wallpapers)" )]
		[TestCase( "SELECT COUNT(*) FROM TEST_WEB_STAT WHERE HOST = 'SYSTEM'" )]
		[TestCase( "SELECT mt.HOST, nmt.HOST, mt.DOMAIN, nmt.DOMAIN FROM TEST_WEB_STAT_MULTI_TENANT mt INNER JOIN TEST_WEB_STAT nmt ON (mt.HOST = nmt.HOST) LIMIT 1" )]
		[TestCase( "SELECT mt.HOST, nmt.HOST, mt.DOMAIN, nmt.DOMAIN FROM TEST_WEB_STAT_MULTI_TENANT mt, TEST_WEB_STAT nmt WHERE mt.HOST = nmt.HOST LIMIT 1" )]
		[TestCase( "SELECT NAME.ALPHA, NAME.BETA, SOME_UIDS.ALPHA, SOME_UIDS.BETA, SOME_UIDS.NAME.TENANT, NAME.INTERACTIONTYPE FROM SOME_UIDS SOME_UIDS LIMIT 20" )]
		// Tests for compound column names (Apache Phoenix SQL)
		[TestCase( @"SELECT PHOENIX_TABLE.A.ID, PHOENIX_TABLE.A.V, PHOENIX_TABLE.A.TIMESTAMP, PHOENIX_TABLE.A.USERID, PHOENIX_TABLE.A.TYPE, PHOENIX_TABLE.PHONEID 
					FROM PHOENIX_TABLE PHOENIX_TABLE ORDER BY PHOENIX_TABLE.A.USERID" )]
		[TestCase( @"SELECT PHOENIX_TABLE.A.ID WHERECLAUSE, PHOENIX_TABLE.A.V, PHOENIX_TABLE.A.TIMESTAMP, PHOENIX_TABLE.A.USERID, 
					PHOENIX_TABLE.A.TYPE, PHOENIX_TABLE.PHONEID 
					FROM PHOENIX_TABLE PHOENIX_TABLE
					WHERE (PHOENIX_TABLE.A.TIMESTAMP>'45') ORDER BY PHOENIX_TABLE.A.TIMESTAMP, PHOENIX_TABLE.A.ID" )]
		[TestCase( @"SELECT PHOENIX_TABLE.A.ID WHERECLAUSE_NOBRACKETS, PHOENIX_TABLE.A.V, PHOENIX_TABLE.A.TIMESTAMP, PHOENIX_TABLE.A.USERID, 
					PHOENIX_TABLE.A.TYPE, PHOENIX_TABLE.PHONEID 
					FROM PHOENIX_TABLE PHOENIX_TABLE
					WHERE PHOENIX_TABLE.A.TIMESTAMP>'45' ORDER BY PHOENIX_TABLE.A.TIMESTAMP, PHOENIX_TABLE.A.ID" )]
		[TestCase( @"SELECT PHOENIX_TABLE.A.ID, PHOENIX_TABLE.A.V, PHOENIX_TABLE.A.TIMESTAMP, PHOENIX_TABLE.A.USERID, 
					PHOENIX_TABLE.A.TYPE, PHOENIX_TABLE.PHONEID 
					FROM PHOENIX_TABLE PHOENIX_TABLE
					JOIN OTHER_TABLE OTHER
					ON OTHER_TABLE.A.B = PHOENIX_TABLE.A.V
					ORDER BY PHOENIX_TABLE.A.TIMESTAMP, PHOENIX_TABLE.A.ID" )]
		[TestCase( @"SELECT SUM(PHOENIX_TABLE.A.ID), PHOENIX_TABLE.A.V, PHOENIX_TABLE.A.TIMESTAMP, PHOENIX_TABLE.A.USERID, 
					PHOENIX_TABLE.A.TYPE, PHOENIX_TABLE.PHONEID 
					FROM PHOENIX_TABLE PHOENIX_TABLE
					GROUP BY PHOENIX_TABLE.A.USERID" )]
		[TestCase( @"SELECT AVG(PHOENIX_TABLE.A.ID), PHOENIX_TABLE.A.V, PHOENIX_TABLE.A.TIMESTAMP, PHOENIX_TABLE.A.USERID, 
					PHOENIX_TABLE.A.TYPE, PHOENIX_TABLE.PHONEID 
					FROM PHOENIX_TABLE PHOENIX_TABLE
					GROUP BY PHOENIX_TABLE.A.USERID
					HAVING PHOENIX_TABLE.A.USERID = 1234" )]
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
