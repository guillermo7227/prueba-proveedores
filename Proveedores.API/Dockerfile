FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:35792ea4ad1db051981f62b313f1be3b46b1f45cadbaa3c288cd0d3056eefb83 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:35792ea4ad1db051981f62b313f1be3b46b1f45cadbaa3c288cd0d3056eefb83 AS build
WORKDIR /src
COPY ["Proveedores.API.csproj", "./"]
RUN dotnet restore "Proveedores.API.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "Proveedores.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Proveedores.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Proveedores.API.dll"]