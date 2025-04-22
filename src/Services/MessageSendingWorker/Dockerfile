FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/MessageSendingWorker/MessageSendingWorker.csproj", "Services/MessageSendingWorker/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "Services/MessageSendingWorker/MessageSendingWorker.csproj"
COPY . .
WORKDIR "/src/Services/MessageSendingWorker"
RUN dotnet build "MessageSendingWorker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MessageSendingWorker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MessageSendingWorker.dll"]
