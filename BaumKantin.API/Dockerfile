#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BaumKantin.API/BaumKantin.API.csproj", "BaumKantin.API/"]
COPY ["BaumKantin.Service/BaumKantin.Service.csproj", "BaumKantin.Service/"]
COPY ["BaumKantin.Repository/BaumKantin.Repository.csproj", "BaumKantin.Repository/"]
COPY ["BaumKantin.Core/BaumKantin.Core.csproj", "BaumKantin.Core/"]
RUN dotnet restore "BaumKantin.API/BaumKantin.API.csproj"
COPY . .
WORKDIR "/src/BaumKantin.API"
RUN dotnet build "BaumKantin.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BaumKantin.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BaumKantin.API.dll"]