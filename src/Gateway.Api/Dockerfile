FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ./src/Gateway.Api/Gateway.csproj ./Gateway.Api/
COPY ./src/Shared/Shared.csproj ./Shared/
RUN dotnet restore ./Gateway.Api/Gateway.csproj

COPY ./src/Gateway.Api/. ./Gateway.Api/
COPY ./src/Shared/. ./Shared/

WORKDIR /src/Gateway.Api
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Gateway.dll"]
