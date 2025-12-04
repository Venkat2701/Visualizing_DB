# Database Schema Visualizer

ASP.NET Core Razor Pages application for visualizing database metadata extracted from Informix 4GL files.

## Features

- **Search & Browse**: Search tables and columns by name with pagination
- **Table Details**: View complete table information including columns, primary keys, foreign keys, and relationships
- **Column Details**: Detailed view of individual columns with constraints and relationships
- **Interactive Navigation**: Click table names to view details, double-click rows for column details
- **Responsive UI**: Bootstrap-based interface that works on all devices

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- Any modern web browser

### Running the Application

1. Clone the repository
2. Navigate to the project directory:
   ```bash
   cd Visualizing_DB/Visualizing_DB
   ```
3. Run the application:
   ```bash
   dotnet run
   ```
4. Open your browser and go to `http://localhost:5159`

## Data Format

Place your JSON schema file at `App_Data/schema.json`. The expected format:

```json
{
  "tables": [
    {
      "table_name": "TableName",
      "containing_file": "file.4gl",
      "columns": [
        {
          "name": "column_name",
          "type": "INTEGER",
          "constraints": ["PRIMARY KEY"]
        }
      ],
      "primary_keys": ["column_name"],
      "foreign_keys": [
        {
          "column": "fk_column",
          "references_table": "OtherTable",
          "references_column": "id"
        }
      ],
      "relationships": [
        {
          "related_table": "OtherTable",
          "relation_type": "foreign_key",
          "via_column": "fk_column"
        }
      ]
    }
  ]
}
```

## Project Structure

- `Models/` - Domain models for JSON deserialization
- `Services/` - Data access layer (SchemaRepository)
- `Pages/Schema/` - Razor Pages for UI
- `App_Data/` - JSON data files

## Technology Stack

- ASP.NET Core 8.0
- Razor Pages
- Bootstrap 5
- System.Text.Json