FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy full solution source code
COPY . .

# Restore and publish
RUN dotnet restore "src/DineProX.DbMigrator/DineProX.DbMigrator.csproj"
RUN dotnet publish "src/DineProX.DbMigrator/DineProX.DbMigrator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DineProX.DbMigrator.dll"]