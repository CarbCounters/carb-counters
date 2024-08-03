param($MigrationName)
dotnet ef migrations add $MigrationName -o Persistance\Migrations --project .\CarbCounter.Infrastructure\ --startup-project .\CarbCounter.WebApi\ --context ApplicationDbContext 