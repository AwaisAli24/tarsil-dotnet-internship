# Database Runner Notes

This project does not rely on `dotnet ef` commands like `Add-Migration` or `Update-Database`. Instead, it uses a custom database runner that applies migration files directly from the app's `db/Migrations` folder.

The runner does this in order:

- checks whether the `schema_migrations` table exists
- scans `db/Migrations/*.sql` files in sorted order
- skips any migration whose filename is already recorded in `schema_migrations`
- executes each unapplied SQL file inside a transaction
- records the migration file name in `schema_migrations` after it succeeds
- runs the seed script afterward to populate initial data

That means the tracking is not "automatic EF tracking" in the usual sense, but it is still automatic tracking from the runner itself. The app records what has already been applied, so reruns do not re-run the same migration.

So the pattern here is: SQL-first migrations, runner-managed execution, and explicit version tracking via `schema_migrations` rather than EF Core migration history.

PostgreSQL ≤ 17
gives 23503, PostgreSQL 18 gives 23001 for RESTRICT, so we catch both
