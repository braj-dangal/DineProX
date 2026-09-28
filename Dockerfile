FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore src/DineProX.HttpApi.Host/DineProX.HttpApi.Host.csproj \
    && dotnet restore src/DineProX.DbMigrator/DineProX.DbMigrator.csproj
RUN dotnet publish src/DineProX.HttpApi.Host/DineProX.HttpApi.Host.csproj \
    --configuration Release --no-restore --output /out/host \
    && dotnet publish src/DineProX.DbMigrator/DineProX.DbMigrator.csproj \
    --configuration Release --no-restore --output /out/migrator

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_HTTP_PORTS=8080 \
    DOTNET_RUNNING_IN_CONTAINER=true
WORKDIR /app
COPY --from=build /out/host ./host
COPY --from=build /out/migrator ./migrator
RUN mkdir -p /app/host/Logs /app/migrator/Logs \
    && chown -R $APP_UID:$APP_UID /app
USER $APP_UID
WORKDIR /app/host
EXPOSE 8080
ENTRYPOINT ["dotnet", "DineProX.HttpApi.Host.dll"]