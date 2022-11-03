#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["app-3/consumer/StreamCluster3.Consumer/StreamCluster3.Consumer.csproj", "app-3/consumer/StreamCluster3.Consumer/"]
RUN dotnet restore "app-3/consumer/StreamCluster3.Consumer/StreamCluster3.Consumer.csproj"
COPY . .
WORKDIR "/src/app-3/consumer/StreamCluster3.Consumer"
RUN dotnet build "StreamCluster3.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StreamCluster3.Consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StreamCluster3.Consumer.dll"]