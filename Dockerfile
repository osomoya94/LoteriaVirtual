FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore Loteria.ApiWeb/Loteria.ApiWeb.csproj
RUN dotnet publish Loteria.ApiWeb/Loteria.ApiWeb.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Loteria.ApiWeb.dll"]