FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution files and restore dependencies
COPY ["src/DineProX.DbMigrator/DineProX.DbMigrator.csproj", "src/DineProX.DbMigrator/"]
COPY ["src/DineProX.Domain/DineProX.Domain.csproj", "src/DineProX.Domain/"]
COPY ["src/DineProX.Domain.Shared/DineProX.Domain.Shared.csproj", "src/DineProX.Domain.Shared/"]
COPY ["src/DineProX.EntityFrameworkCore/DineProX.EntityFrameworkCore.csproj", "src/DineProX.EntityFrameworkCore/"]
RUN dotnet restore "src/DineProX.DbMigrator/DineProX.DbMigrator.csproj"

# Copy full source and publish
COPY . .
WORKDIR "/src/src/DineProX.DbMigrator"
RUN dotnet publish "DineProX.DbMigrator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DineProX.DbMigrator.dll"]