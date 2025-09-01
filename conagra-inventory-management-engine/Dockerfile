FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["conagra-inventory-management-engine/conagra-inventory-management-engine.csproj", "conagra-inventory-management-engine/"]
RUN dotnet restore "conagra-inventory-management-engine/conagra-inventory-management-engine.csproj"
COPY . .
WORKDIR "/src/conagra-inventory-management-engine"
RUN dotnet build "conagra-inventory-management-engine.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "conagra-inventory-management-engine.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "conagra-inventory-management-engine.dll"]
