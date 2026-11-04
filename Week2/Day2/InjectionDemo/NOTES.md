# SQL Injection Notes

In `Program.cs`, the unsafe example builds the SQL query by putting the user input directly into the string.

For example, it might look like this:

```csharp
var name = userInput;
var sql = $"SELECT * FROM Users WHERE Name = '{name}'";
```

This is dangerous because the database sees the final SQL as a real command, not just a value. If a user types something like:

```text
' OR '1'='1
```

then the query becomes:

```sql
SELECT * FROM Users WHERE Name = '' OR '1'='1'
```

That condition is always true, so the database can return every row. This is SQL injection: the attacker is modifying the query logic by entering special SQL characters.

The safer version uses parameters instead of string interpolation:

```csharp
var sql = "SELECT * FROM Users WHERE Name = @Name";
cmd.Parameters.AddWithValue("@Name", name);
```

Here, the SQL text is fixed and the user input is sent as a parameter value. The database treats it as data, not as executable SQL. So even if someone enters quotes or SQL syntax, it is not interpreted as code. It is just treated as a normal search value.

That is why parameters fix the problem: they separate the query structure from the user data. The database still knows what the query is supposed to do, but it never mixes untrusted input into the actual SQL command itself.
