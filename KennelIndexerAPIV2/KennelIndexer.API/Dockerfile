#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["KennelIndexer.API/KennelIndexer.API.csproj", "KennelIndexer.API/"]
RUN dotnet restore "KennelIndexer.API/KennelIndexer.API.csproj"
COPY . .
WORKDIR "/src/KennelIndexer.API"
RUN dotnet build "KennelIndexer.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KennelIndexer.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copy the Resources directory
COPY ["KennelIndexer.API/Resources", "Resources"]

ENTRYPOINT ["dotnet", "KennelIndexer.API.dll"]