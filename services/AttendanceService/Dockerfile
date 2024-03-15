FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["services/AttendanceService/AttendanceService.csproj", "services/AttendanceService/"]
COPY ["shared/ApplicationBase/ApplicationBase.csproj", "shared/ApplicationBase/"]
COPY ["shared/Contracts/Contracts.csproj", "shared/Contracts/"]
RUN dotnet restore "services/AttendanceService/AttendanceService.csproj"
COPY . .
WORKDIR "/src/services/AttendanceService"
RUN dotnet build "AttendanceService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AttendanceService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
ENV ASPNETCORE_HTTP_PORTS=80
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AttendanceService.dll"]
