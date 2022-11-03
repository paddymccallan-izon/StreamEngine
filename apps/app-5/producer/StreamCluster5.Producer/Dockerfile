#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["app-5/producer/StreamCluster5.Producer/StreamCluster5.Producer.csproj", "app-5/producer/StreamCluster5.Producer/"]
RUN dotnet restore "app-5/producer/StreamCluster5.Producer/StreamCluster5.Producer.csproj"
COPY . .
WORKDIR "/apps/app-5/producer/StreamCluster5.Producer"
RUN dotnet build "StreamCluster5.Producer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StreamCluster5.Producer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StreamCluster5.Producer.dll"]