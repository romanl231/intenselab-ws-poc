FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копіюємо .csproj окремо для кешування
COPY ./src/Realtime.Ws/WebSocket.csproj ./Realtime.Ws/
COPY ./src/Shared/Shared.csproj ./Shared/
RUN dotnet restore ./Realtime.Ws/WebSocket.csproj

# Копіюємо решту проєкту
COPY ./src/Realtime.Ws/. ./Realtime.Ws/
COPY ./src/Shared/. ./Shared/

# Публікуємо
WORKDIR /src/Realtime.Ws
RUN dotnet publish WebSocket.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "WebSocket.dll"]
