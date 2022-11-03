#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["app-4/producer/StreamCluster4.Producer/StreamCluster4.Producer.csproj", "app-4/producer/StreamCluster4.Producer/"]
RUN dotnet restore "app-4/producer/StreamCluster4.Producer/StreamCluster4.Producer.csproj"
COPY . .
WORKDIR "/src/app-4/producer/StreamCluster4.Producer"
RUN dotnet build "StreamCluster4.Producer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StreamCluster4.Producer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StreamCluster4.Producer.dll"]