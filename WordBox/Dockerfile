FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WordBox/WordBox.csproj", "WordBox/"]
RUN dotnet restore "WordBox/WordBox.csproj"
COPY . .
WORKDIR "/src/WordBox"
RUN dotnet build "WordBox.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WordBox.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WordBox.dll"]
