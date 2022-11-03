FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["app-1/producer/StreamCluster1.Producer/StreamCluster1.Producer.csproj", "app-1/producer/StreamCluster1.Producer/"]
RUN dotnet restore "app-1/producer/StreamCluster1.Producer/StreamCluster1.Producer.csproj"
COPY . .
WORKDIR "/apps/app-1/producer/StreamCluster1.Producer"
RUN dotnet build "StreamCluster1.Producer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StreamCluster1.Producer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StreamCluster1.Producer.dll"]