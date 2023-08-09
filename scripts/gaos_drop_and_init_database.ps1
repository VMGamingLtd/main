
Push-Location ..\gaos
dotnet ef database drop --force
dotnet ef database update
Pop-Location
