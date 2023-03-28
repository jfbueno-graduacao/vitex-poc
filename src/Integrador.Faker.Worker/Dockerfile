FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

COPY . ./

RUN dotnet restore
RUN dotnet publish -c Release -o bin

FROM mcr.microsoft.com/dotnet/runtime:7.0
WORKDIR /app
COPY --from=build-env /app/bin .

RUN sed -i 's/TLSv1.2/TLSv1.0/g' /etc/ssl/openssl.cnf

ENTRYPOINT ["dotnet", "Integrador.Faker.Worker.dll"]