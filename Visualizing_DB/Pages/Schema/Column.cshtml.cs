using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Visualizing_DB.Models;
using Visualizing_DB.Services;

namespace Visualizing_DB.Pages.Schema;

public class ColumnModel : PageModel
{
    private readonly SchemaRepository _repository;

    public ColumnModel(SchemaRepository repository)
    {
        _repository = repository;
    }

    [BindProperty(SupportsGet = true)]
    public string Table { get; set; } = "";

    [BindProperty(SupportsGet = true)]
    public string Column { get; set; } = "";

    public TableInfo? TableInfo { get; set; }
    public ColumnInfo? ColumnInfo { get; set; }
    public IEnumerable<ForeignKeyInfo> ColumnForeignKeys { get; set; } = Enumerable.Empty<ForeignKeyInfo>();
    public IEnumerable<RelationshipInfo> ColumnRelationships { get; set; } = Enumerable.Empty<RelationshipInfo>();
    public IEnumerable<(TableInfo Table, ForeignKeyInfo ForeignKey)> ColumnReferences { get; set; } = Enumerable.Empty<(TableInfo, ForeignKeyInfo)>();

    public IActionResult OnGet()
    {
        if (string.IsNullOrWhiteSpace(Table) || string.IsNullOrWhiteSpace(Column))
            return NotFound();

        var result = _repository.GetColumn(Table, Column);
        if (result == null)
            return NotFound();

        (TableInfo, ColumnInfo) = result.Value;

        ColumnForeignKeys = TableInfo.ForeignKeys.Where(fk => 
            string.Equals(fk.Column, Column, StringComparison.OrdinalIgnoreCase));

        ColumnRelationships = TableInfo.Relationships.Where(rel => 
            string.Equals(rel.ViaColumn, Column, StringComparison.OrdinalIgnoreCase));

        ColumnReferences = _repository.GetColumnReferences(TableInfo.TableName, ColumnInfo.Name);

        return Page();
    }
}