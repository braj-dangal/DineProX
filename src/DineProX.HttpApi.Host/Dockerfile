FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy full solution source code
COPY . .

# Restore and publish
RUN dotnet restore "src/DineProX.HttpApi.Host/DineProX.HttpApi.Host.csproj"
RUN dotnet publish "src/DineProX.HttpApi.Host/DineProX.HttpApi.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DineProX.HttpApi.Host.dll"]