using System.Text.Json;
using Visualizing_DB.Models;

namespace Visualizing_DB.Services;

public class SchemaRepository
{
    private readonly SchemaRoot _schema;
    private readonly ILogger<SchemaRepository> _logger;

    public SchemaRepository(IWebHostEnvironment env, ILogger<SchemaRepository> logger)
    {
        _logger = logger;
        _schema = LoadSchema(env);
    }

    private SchemaRoot LoadSchema(IWebHostEnvironment env)
    {
        try
        {
            var filePath = Path.Combine(env.ContentRootPath, "App_Data", "schema.json");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning("Schema file not found at {FilePath}", filePath);
                return new SchemaRoot();
            }

            var json = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            return JsonSerializer.Deserialize<SchemaRoot>(json, options) ?? new SchemaRoot();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load schema file");
            return new SchemaRoot();
        }
    }

    public IEnumerable<TableInfo> GetTables() => _schema.Tables;

    public TableInfo? GetTable(string tableName) =>
        _schema.Tables.FirstOrDefault(t => string.Equals(t.TableName, tableName, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<(TableInfo Table, ColumnInfo Column)> SearchColumns(string? searchTerm)
    {
        var query = _schema.Tables.SelectMany(table => 
            table.Columns.Select(column => (Table: table, Column: column)));

        if (string.IsNullOrWhiteSpace(searchTerm))
            return query;

        return query.Where(item =>
            item.Table.TableName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            item.Column.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
    }

    public (TableInfo Table, ColumnInfo Column)? GetColumn(string tableName, string columnName)
    {
        var table = GetTable(tableName);
        if (table == null) return null;

        var column = table.Columns.FirstOrDefault(c => 
            string.Equals(c.Name, columnName, StringComparison.OrdinalIgnoreCase));
        
        return column != null ? (table, column) : null;
    }

    public IEnumerable<string> GetRelatedTables(TableInfo table)
    {
        var related = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        
        foreach (var fk in table.ForeignKeys)
            related.Add(fk.ReferencesTable);
        
        foreach (var rel in table.Relationships)
            related.Add(rel.RelatedTable);
        
        return related;
    }
}