FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files
COPY ["Library.Api/Library.Api.csproj", "Library.Api/"]
RUN dotnet restore "Library.Api/Library.Api.csproj"

# Copy source code
COPY ["Library.Api/", "Library.Api/"]

# Build
WORKDIR "/src/Library.Api"
RUN dotnet build "Library.Api.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "Library.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 8080
EXPOSE 8443

ENTRYPOINT ["dotnet", "Library.Api.dll"]
