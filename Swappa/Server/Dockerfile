#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Swappa/Server/Swappa.Server.csproj", "Swappa/Server/"]
COPY ["Swappa/Client/Swappa.Client.csproj", "Swappa/Client/"]
COPY ["Swappa/Shared/Swappa.Shared.csproj", "Swappa/Shared/"]
COPY ["Swappa.Entities/Swappa.Entities.csproj", "Swappa.Entities/"]
COPY ["Swappa.Data/Swappa.Data.csproj", "Swappa.Data/"]
RUN dotnet restore "Swappa/Server/Swappa.Server.csproj"
COPY . .
WORKDIR "/src/Swappa/Server"
RUN dotnet build "Swappa.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Swappa.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Swappa.Server.dll"]