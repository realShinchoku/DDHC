FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["services/OWAdministrativeService/OWAdministrativeService.csproj", "services/OWAdministrativeService/"]
COPY ["shared/ApplicationBase/ApplicationBase.csproj", "shared/ApplicationBase/"]
COPY ["shared/Contracts/Contracts.csproj", "shared/Contracts/"]
RUN dotnet restore "services/OWAdministrativeService/OWAdministrativeService.csproj"
COPY . .
WORKDIR "/src/services/OWAdministrativeService"
RUN dotnet build "OWAdministrativeService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OWAdministrativeService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
ENV ASPNETCORE_HTTP_PORTS=80
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OWAdministrativeService.dll"]
