FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/DineProX.HttpApi.Host/DineProX.HttpApi.Host.csproj", "src/DineProX.HttpApi.Host/"]
COPY ["src/DineProX.Application/DineProX.Application.csproj", "src/DineProX.Application/"]
COPY ["src/DineProX.Application.Contracts/DineProX.Application.Contracts.csproj", "src/DineProX.Application.Contracts/"]
COPY ["src/DineProX.Domain/DineProX.Domain.csproj", "src/DineProX.Domain/"]
COPY ["src/DineProX.Domain.Shared/DineProX.Domain.Shared.csproj", "src/DineProX.Domain.Shared/"]
COPY ["src/DineProX.EntityFrameworkCore/DineProX.EntityFrameworkCore.csproj", "src/DineProX.EntityFrameworkCore/"]
COPY ["src/DineProX.HttpApi/DineProX.HttpApi.csproj", "src/DineProX.HttpApi/"]
COPY ["common.props", "."]
COPY ["NuGet.Config", "."]
COPY ["DineProX.sln", "."]

RUN dotnet restore DineProX.sln

COPY . .
RUN dotnet publish src/DineProX.HttpApi.Host/DineProX.HttpApi.Host.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "DineProX.HttpApi.Host.dll"]
