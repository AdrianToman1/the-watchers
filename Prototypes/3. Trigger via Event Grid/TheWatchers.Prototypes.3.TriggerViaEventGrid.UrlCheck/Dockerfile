#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated8.0 AS base
WORKDIR /home/site/wwwroot
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TheWatchers.Prototypes.3.TriggerViaEventGrid.UrlCheck/TheWatchers.Prototypes.3.TriggerViaEventGrid.UrlCheck.csproj", "TheWatchers.Prototypes.3.TriggerViaEventGrid.UrlCheck/"]
RUN dotnet restore "./TheWatchers.Prototypes.3.TriggerViaEventGrid.UrlCheck/TheWatchers.Prototypes.3.TriggerViaEventGrid.UrlCheck.csproj"
COPY . .
WORKDIR "/src/TheWatchers.Prototypes.3.TriggerViaEventGrid.UrlCheck"
RUN dotnet build "./TheWatchers.Prototypes.3.TriggerViaEventGrid.UrlCheck.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TheWatchers.Prototypes.3.TriggerViaEventGrid.UrlCheck.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true