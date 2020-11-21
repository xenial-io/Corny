@pushd %~dp0
@dotnet run --project ".\build\build\build.csproj" -- %*
@popd