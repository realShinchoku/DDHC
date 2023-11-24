FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["services/StudentService/StudentService.csproj", "services/StudentService/"]
COPY ["shareds/ApplicationBase/ApplicationBase.csproj", "shareds/ApplicationBase/"]
COPY ["shareds/Contracts/Contracts.csproj", "shareds/Contracts/"]
RUN dotnet restore "services/StudentService/StudentService.csproj"
COPY . .
WORKDIR "/src/services/StudentService"
RUN dotnet build "StudentService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StudentService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudentService.dll"]
