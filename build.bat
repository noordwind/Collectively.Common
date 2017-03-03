dotnet restore --source "https://api.nuget.org/v3/index.json" --source "https://www.myget.org/F/collectively%build-env%/api/v3/index.json" --no-cache
dotnet pack "Collectively.Common" -o .
