FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["gateways/MobileGateway/MobileGateway.csproj", "gateways/MobileGateway/"]
COPY ["shared/ApplicationBase/ApplicationBase.csproj", "shared/ApplicationBase/"]
RUN dotnet restore "gateways/MobileGateway/MobileGateway.csproj"
COPY . .
WORKDIR "/src/gateways/MobileGateway"
RUN dotnet build "MobileGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MobileGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
ENV ASPNETCORE_HTTP_PORTS=80
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MobileGateway.dll"]
