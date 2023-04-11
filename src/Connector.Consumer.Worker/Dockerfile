FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

COPY ./Connector.Consumer.Worker/Connector.Consumer.Worker.csproj ./Connector.Consumer.Worker/Connector.Consumer.Worker.csproj
COPY ./Connector.Common/Connector.Common.csproj ./Connector.Common/Connector.Common.csproj

WORKDIR /app/Connector.Consumer.Worker
RUN dotnet restore

WORKDIR /app
COPY ./Connector.Consumer.Worker/. ./Connector.Consumer.Worker/
COPY ./Connector.Common/. ./Connector.Common/

WORKDIR /app/Connector.Consumer.Worker
RUN dotnet publish -c Release -o /app/bin

FROM mcr.microsoft.com/dotnet/runtime:7.0
WORKDIR /app
COPY --from=build-env /app/bin .

ENTRYPOINT ["dotnet", "Connector.Consumer.Worker.dll"]