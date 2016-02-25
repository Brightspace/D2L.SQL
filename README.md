# D2L.SQL

[![Build status](https://ci.appveyor.com/api/projects/status/7mshe7enqhr12037/branch/master?svg=true)](https://ci.appveyor.com/project/Brightspace/d2l-sql/branch/master)

C# libraries for handling SQL.

## Libraries

### D2L.SQL.Validation

Policy enforcement on untrusted SQL.

Usage:

`IValidator validator = new ReadOnlyValidator();`

`string safeSql = validator.Sanitize( sql );`

Throws D2L.SQL.Validation.SqlValidationException if the sql is unsafe or unparseable for some other reason.

## Contributing

1. **Fork** the repository. Committing directly against this repository is
   highly discouraged.

2. Make your modifications in a branch, updating and writing new tests.

3. Ensure that all tests pass

4. `rebase` your changes against master. *Do not merge*.

5. Submit a pull request to this repository. Wait for tests to run and someone
   to chime in.

## Implementation Notes

The ReadOnlyValidator attempts to parse the input using a subset of SQL defined using [Irony](https://irony.codeplex.com/), and throws if it fails.

The grammar only covers SELECT statements, which guarantees no data will be modified by executing the sql. It also omits qualified tablenames of the form SCHEMA.TABLE,
so only tables in the default schema/tablespace are usable.

### Why Irony?

There are few SQL parsers available for .NET, and using a parser library like Irony has the key advantage that we can define only the subset of SQL that we want to accept. Irony is the
only native C# parser library in common use, and while its documentation is sparse, it is reasonably well-supported via discussions.

Getting a complete grammar would be achievable in less code by using an existing parser, such as the Apache Phoenix jdbc driver (via jni), or the MS sql parser. However, we would then
have to write code to traverse the resulting parse tree, and ensure that no branches are present that might alter data. This approach lets us effectively whitelist the valid expressions
instead.

Additionally, Irony is elegant enough that the bulk of the code here is simply declarative statements contained in a single class.


