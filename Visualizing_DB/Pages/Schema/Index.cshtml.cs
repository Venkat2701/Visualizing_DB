using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Visualizing_DB.Models;
using Visualizing_DB.Services;

namespace Visualizing_DB.Pages.Schema;

public class IndexModel : PageModel
{
    private readonly SchemaRepository _repository;

    public IndexModel(SchemaRepository repository)
    {
        _repository = repository;
    }

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    [BindProperty(SupportsGet = true)]
    public int PageNumber { get; set; } = 1;

    [BindProperty(SupportsGet = true)]
    public int PageSize { get; set; } = 20;

    public PagedResult<(TableInfo Table, ColumnInfo Column)> Results { get; set; } = null!;

    public void OnGet()
    {
        var columns = _repository.SearchColumns(SearchTerm);
        Results = new PagedResult<(TableInfo, ColumnInfo)>(columns, PageNumber, PageSize);
    }
}