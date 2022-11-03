#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["app-4/consumer/StreamCluster4.Consumer/StreamCluster4.Consumer.csproj", "app-4/consumer/StreamCluster4.Consumer/"]
RUN dotnet restore "app-4/consumer/StreamCluster4.Consumer/StreamCluster4.Consumer.csproj"
COPY . .
WORKDIR "/src/app-4/consumer/StreamCluster4.Consumer"
RUN dotnet build "StreamCluster4.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StreamCluster4.Consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StreamCluster4.Consumer.dll"]