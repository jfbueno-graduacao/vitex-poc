FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

COPY ./Connector.Producer.Worker/Connector.Producer.Worker.csproj ./Connector.Producer.Worker/Connector.Producer.Worker.csproj
COPY ./Connector.Common/Connector.Common.csproj ./Connector.Common/Connector.Common.csproj

WORKDIR /app/Connector.Producer.Worker
RUN dotnet restore

WORKDIR /app
COPY ./Connector.Producer.Worker/. ./Connector.Producer.Worker/
COPY ./Connector.Common/. ./Connector.Common/

WORKDIR /app/Connector.Producer.Worker
RUN dotnet publish -c Release -o /app/bin

FROM mcr.microsoft.com/dotnet/runtime:7.0
WORKDIR /app
COPY --from=build-env /app/bin .

ENTRYPOINT ["dotnet", "Connector.Producer.Worker.dll"]