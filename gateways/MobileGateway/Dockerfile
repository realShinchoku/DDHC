FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["gateways/MobileGateway/MobileGateway.csproj", "gateways/MobileGateway/"]
COPY ["shareds/ApplicationBase/ApplicationBase.csproj", "shareds/ApplicationBase/"]
RUN dotnet restore "gateways/MobileGateway/MobileGateway.csproj"
COPY . .
WORKDIR "/src/gateways/MobileGateway"
RUN dotnet build "MobileGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MobileGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MobileGateway.dll"]
