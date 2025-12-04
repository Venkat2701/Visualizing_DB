using System.Text.Json.Serialization;

namespace Visualizing_DB.Models;

public class SchemaRoot
{
    [JsonPropertyName("tables")]
    public List<TableInfo> Tables { get; set; } = new();
}

public class TableInfo
{
    [JsonPropertyName("table_name")]
    public string TableName { get; set; } = "";

    [JsonPropertyName("containing_file")]
    public string? ContainingFile { get; set; }

    [JsonPropertyName("columns")]
    public List<ColumnInfo> Columns { get; set; } = new();

    [JsonPropertyName("primary_keys")]
    public List<string> PrimaryKeys { get; set; } = new();

    [JsonPropertyName("foreign_keys")]
    public List<ForeignKeyInfo> ForeignKeys { get; set; } = new();

    [JsonPropertyName("relationships")]
    public List<RelationshipInfo> Relationships { get; set; } = new();
}

public class ColumnInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("type")]
    public string Type { get; set; } = "";

    [JsonPropertyName("constraints")]
    public List<string> Constraints { get; set; } = new();
}

public class ForeignKeyInfo
{
    [JsonPropertyName("column")]
    public string Column { get; set; } = "";

    [JsonPropertyName("references_table")]
    public string ReferencesTable { get; set; } = "";

    [JsonPropertyName("references_column")]
    public string ReferencesColumn { get; set; } = "";
}

public class RelationshipInfo
{
    [JsonPropertyName("related_table")]
    public string RelatedTable { get; set; } = "";

    [JsonPropertyName("relation_type")]
    public string RelationType { get; set; } = "";

    [JsonPropertyName("via_column")]
    public string ViaColumn { get; set; } = "";
}