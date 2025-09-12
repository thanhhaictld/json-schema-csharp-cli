# auto increment version
dotnet pack ./JsonSchemaToCSharpCli/JsonSchemaToCSharpCli.csproj --configuration Release /p:PackageVersion=1.0.1
dotnet tool uninstall --global JsonSchemaToCSharpCli
dotnet tool install --global --add-source ./JsonSchemaToCSharpCli/bin/Release JsonSchemaToCSharpCli