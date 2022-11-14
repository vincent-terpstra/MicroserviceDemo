FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PlatformService/PlatformService.csproj", "PlatformService/"]
RUN dotnet restore "PlatformService/PlatformService.csproj"
COPY . .
WORKDIR "/src/PlatformService"
RUN dotnet build "PlatformService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PlatformService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PlatformService.dll"]
