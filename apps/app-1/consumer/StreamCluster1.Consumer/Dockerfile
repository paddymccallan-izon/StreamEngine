FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["app-1/consumer/StreamCluster1.Consumer/StreamCluster1.Consumer.csproj", "app-1/consumer/StreamCluster1.Consumer/"]
RUN dotnet restore "app-1/consumer/StreamCluster1.Consumer/StreamCluster1.Consumer.csproj"
COPY . .
WORKDIR "/apps/app-1/consumer/StreamCluster1.Consumer"
RUN dotnet build "StreamCluster1.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StreamCluster1.Consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StreamCluster1.Consumer.dll"]