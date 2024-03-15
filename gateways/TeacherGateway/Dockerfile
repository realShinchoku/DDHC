FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["gateways/TeacherGateway/TeacherGateway.csproj", "gateways/TeacherGateway/"]
COPY ["shared/ApplicationBase/ApplicationBase.csproj", "shared/ApplicationBase/"]
RUN dotnet restore "gateways/TeacherGateway/TeacherGateway.csproj"
COPY . .
WORKDIR "/src/gateways/TeacherGateway"
RUN dotnet build "TeacherGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TeacherGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
ENV ASPNETCORE_HTTP_PORTS=80
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeacherGateway.dll"]
