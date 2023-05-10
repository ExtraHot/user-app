FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["vk-testing/vk-testing.csproj", "vk-testing/"]
RUN dotnet restore "vk-testing/vk-testing.csproj"
COPY . .
WORKDIR "/src/vk-testing"
RUN dotnet build "vk-testing.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "vk-testing.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "vk-testing.dll"]