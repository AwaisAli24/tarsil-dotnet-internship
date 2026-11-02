using Npgsql;

namespace LibraryApi.Data;

public class DatabaseMigrator
{
    private readonly NpgsqlDataSource _datasource;
    public DatabaseMigrator(NpgsqlDataSource datasource)
    {
        _datasource = datasource;
    }
    private async Task EnsureMigrationTableExistAsync()
    {
        string sql = """
            CREATE TABLE IF NOT EXISTS schema_migrations(
            version VARCHAR(255) PRIMARY KEY,
            applied_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
            );
        """;
        await using var command = _datasource.CreateCommand(sql);
        await command.ExecuteNonQueryAsync();
    }
    public async Task RunAsync()
    {
        await EnsureMigrationTableExistAsync();

        string[] migrationFiles = GetMigrationFilesAsync();

        foreach (var migrationPath in migrationFiles)
        {
            if (await isMigrationAppliedAsync(Path.GetFileName(migrationPath)))
            {
                continue;
            }
            await ApplyMigrationAsync(migrationPath);
        }

    }
    private string[] GetMigrationFilesAsync()
    {
        string migrationDirectory = Path.Combine(
            AppContext.BaseDirectory,
            "db",
            "migrations"
        );

        if (!Directory.Exists(migrationDirectory))
        {
            throw new DirectoryNotFoundException(
                $"Migration directory not found: {migrationDirectory}"
            );
        }

        string[] files = Directory.GetFiles(
            migrationDirectory,
            "*.sql"
        );

        Array.Sort(files, StringComparer.Ordinal);

        if (files.Length == 0)
        {
            throw new InvalidOperationException(
                "No migration files were found."
            );
        }

        return files;
    }
    private async Task<bool> isMigrationAppliedAsync(string filename)
    {
        string sql = """
            SELECT EXISTS(
                SELECT 1 FROM schema_migrations
                WHERE version = @version
            )
        """;
        await using var command = _datasource.CreateCommand(sql);
        command.Parameters.AddWithValue("@version", filename);
        bool exists = (bool)(await command.ExecuteScalarAsync())!;
        return exists;
    }
    private async Task ApplyMigrationAsync(string migrationPath)
    {
        string filename = Path.GetFileName(migrationPath);
        string sql = await File.ReadAllTextAsync(migrationPath);
        await using var connection = await _datasource.OpenConnectionAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            await using var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = sql;
            await command.ExecuteNonQueryAsync();
            command.Parameters.Clear();
            command.CommandText = """
                INSERT INTO schema_migrations(version)
                VALUES(@version);
            """;
            command.Parameters.AddWithValue("version", filename);
            await command.ExecuteNonQueryAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

    }
}