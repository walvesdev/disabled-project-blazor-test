#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5004
EXPOSE 5005

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Sindvamb.HangFire.Host/Sindvamb.HangFire.Host.csproj", "Sindvamb.HangFire.Host/"]
COPY ["Sindvamb.Application.Core/Sindvamb.Application.Core.csproj", "Sindvamb.Application.Core/"]
COPY ["Sindvamb.Caching/Sindvamb.Caching.csproj", "Sindvamb.Caching/"]
COPY ["Sindvamb.Utils/Sindvamb.Utils.csproj", "Sindvamb.Utils/"]
COPY ["Sindvamb.Files/Sindvamb.Files.csproj", "Sindvamb.Files/"]
COPY ["Sindvamb.Security/Sindvamb.Security.csproj", "Sindvamb.Security/"]
COPY ["Sindvamb.Events/Sindvamb.Events.csproj", "Sindvamb.Events/"]
COPY ["Sindvamb.Domain/Sindvamb.Domain.csproj", "Sindvamb.Domain/"]
COPY ["Sindvamb.Repository/Sindvamb.Repository.csproj", "Sindvamb.Repository/"]
COPY ["Sindvamb.Templates/Sindvamb.Templates.csproj", "Sindvamb.Templates/"]
COPY ["Sindvamb.Service/Sindvamb.Service.csproj", "Sindvamb.Service/"]
COPY ["Sindvamb.HangFire.Core/Sindvamb.HangFire.Core.csproj", "Sindvamb.HangFire.Core/"]
COPY ["Sindvamb.HangFire.Jobs/Sindvamb.HangFire.Jobs.csproj", "Sindvamb.HangFire.Jobs/"]
RUN dotnet restore "./Sindvamb.HangFire.Host/Sindvamb.HangFire.Host.csproj"
COPY . .
WORKDIR "/src/Sindvamb.HangFire.Host"
RUN dotnet build "./Sindvamb.HangFire.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Sindvamb.HangFire.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sindvamb.HangFire.Host.dll"]