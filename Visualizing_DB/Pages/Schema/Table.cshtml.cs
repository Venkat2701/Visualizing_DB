using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Visualizing_DB.Models;
using Visualizing_DB.Services;

namespace Visualizing_DB.Pages.Schema;

public class TableModel : PageModel
{
    private readonly SchemaRepository _repository;

    public TableModel(SchemaRepository repository)
    {
        _repository = repository;
    }

    [BindProperty(SupportsGet = true)]
    public string Name { get; set; } = "";

    public TableInfo? Table { get; set; }
    public IEnumerable<string> RelatedTables { get; set; } = Enumerable.Empty<string>();
    public IEnumerable<(TableInfo Table, ForeignKeyInfo ForeignKey)> TableReferences { get; set; } = Enumerable.Empty<(TableInfo, ForeignKeyInfo)>();

    public IActionResult OnGet()
    {
        if (string.IsNullOrWhiteSpace(Name))
            return NotFound();

        Table = _repository.GetTable(Name);
        if (Table == null)
            return NotFound();

        RelatedTables = _repository.GetRelatedTables(Table);
        TableReferences = _repository.GetTableReferences(Table.TableName);
        return Page();
    }
}