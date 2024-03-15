FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["gateways/P7Gateway/P7Gateway.csproj", "gateways/P7Gateway/"]
COPY ["shared/ApplicationBase/ApplicationBase.csproj", "shared/ApplicationBase/"]
RUN dotnet restore "gateways/P7Gateway/P7Gateway.csproj"
COPY . .
WORKDIR "/src/gateways/P7Gateway"
RUN dotnet build "P7Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "P7Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
ENV ASPNETCORE_HTTP_PORTS=80
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "P7Gateway.dll"]
