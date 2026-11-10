using Npgsql;

namespace SchoolApi.Data;

public class DataBaseMigrator
{
    private readonly NpgsqlDataSource _source;
    public DataBaseMigrator(NpgsqlDataSource source)
    {
        _source = source;
    }
    public async Task RunAsync()
    {
        await EnsureMigrationTablesExist();
        string[] files = GetMigrationFiles();
        foreach (var file in files)
        {
            if (await isMigrationApplied(Path.GetFileName(file)))
            {
                continue;
            }
            await ApplyMigration(file);
        }
    }
    private async Task EnsureMigrationTablesExist()
    {
        string sql = """
            CREATE TABLE IF NOT EXISTS schema_migrations(
                version TEXT NOT NULL PRIMARY KEY,
                applied_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
            )
        """;
        using var command = _source.CreateCommand(sql);
        await command.ExecuteNonQueryAsync();
    }
    private string[] GetMigrationFiles()
    {
        string migrationPath = Path.Combine(AppContext.BaseDirectory, "db", "Migrations");

        if (!Directory.Exists(migrationPath))
        {
            throw new Exception($"Directory does not exist.{migrationPath}");
        }
        string[] files = Directory.GetFiles(migrationPath);

        if (files.Length == 0)
        {
            throw new Exception("No files exist.");
        }
        return files;
    }
    private async Task<bool> isMigrationApplied(string filename)
    {
        using var command = _source.CreateCommand("""
            SELECT EXISTS (
                SELECT 1 FROM schema_migrations
                WHERE version = @version
            );
        """);
        command.Parameters.AddWithValue("@version", filename);
        bool exist = (bool)(await command.ExecuteScalarAsync())!;
        return exist;
    }
    private async Task ApplyMigration(string migrationPath)
    {
        var filename = Path.GetFileName(migrationPath);
        string sql = await File.ReadAllTextAsync(migrationPath);
        await using var connection = await _source.OpenConnectionAsync();
        await using var transaction = await connection.BeginTransactionAsync();
        try
        {
            using var command = connection.CreateCommand();
            command.CommandText = sql;
            command.Transaction = transaction;
            await command.ExecuteNonQueryAsync();
            command.CommandText = """
                INSERT INTO schema_migrations(version)
                VALUES(@filename);
            """;
            command.Parameters.AddWithValue("@filename", filename);
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