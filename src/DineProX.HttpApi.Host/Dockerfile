FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files and restore
COPY ["src/DineProX.HttpApi.Host/DineProX.HttpApi.Host.csproj", "src/DineProX.HttpApi.Host/"]
# Copy other project references here...
RUN dotnet restore "src/DineProX.HttpApi.Host/DineProX.HttpApi.Host.csproj"

COPY . .
WORKDIR "/src/src/DineProX.HttpApi.Host"
RUN dotnet publish "DineProX.HttpApi.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DineProX.HttpApi.Host.dll"]