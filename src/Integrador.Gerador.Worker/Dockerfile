# TODO: Incompleto - Revisar todo

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Integrador.Gerador.Worker.csproj", "background-service/"]
RUN dotnet restore "Integrador.Gerador.Worker.csproj"
COPY . .
WORKDIR "/src/Integrador.Gerador.Worker"
RUN dotnet build "App.WorkerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "App.WorkerService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "App.WorkerService.dll"]