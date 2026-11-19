# Executar na pasta root do projeto:

# docker build -t sindvamb.web.wasm:latest .
# docker build . -t sindvamb.web.wasm:latest  --no-cache -f Web/Sindvamb.Web/Dockerfile
# docker run -p 8080:80 sindvamb.web.wasm:latest

# ou

# docker-compose -f .\docker-compose-wasm.yml up -d

# continuação 

# docker login registry.sindvamb.com.br -u sindvamb -p 1q2w3E*@
# docker tag sindvamb.web.wasm:latest registry.sindvamb.com.br/sindvamb.web.wasm:latest
# docker push registry.sindvamb.com.br/sindvamb.web.wasm:latest

# docker-compose -f .\compose-wasm.yml down

# docker exec registry /bin/registry garbage-collect   /etc/docker/registry/config.yml --delete-untagged
# curl -u sindvamb:1q2w3E*@ -sS -X DELETE https://registry.sindvamb.com.br/v2/sindvamb.application.api/manifests/sha256:a8e2f78ab30dbe82aba926798b38ebbad7cb0afb1480b5aa0dc2427e5b7258ba
# docker exec registry bin/registry garbage-collect --dry-run /etc/docker/registry/config.yml
# curl -u sindvamb:1q2w3E*@ -sS https://registry.sindvamb.com.br/v2/sindvamb.application.api/tags/list
# curl -u sindvamb:1q2w3E*@ -sS -X DELETE https://registry.sindvamb.com.br/v2/sindvamb.application.api/manifests/952d4cd4416ddbab5ed4bfaca7111a2ad2f40659738d12c6f37ff1a920c9094a

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Web/Sindvamb.Web/Sindvamb.Web.csproj", "Web/Sindvamb.Web/"]
COPY ["Web/Sindvamb.Web.Shared/Sindvamb.Web.Shared.csproj", "Web/Sindvamb.Web.Shared/"]
COPY ["Sindvamb.Domain/Sindvamb.Domain.csproj", "Sindvamb.Domain/"]
RUN dotnet restore "Web/Sindvamb.Web/Sindvamb.Web.csproj"
COPY . .
WORKDIR "/src/Web/Sindvamb.Web"
RUN dotnet build "Sindvamb.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Sindvamb.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish --self-contained true --no-restore /p:PublishTrimmed=true /p:UseAppHost=false

FROM nginx:alpine as final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY --from=publish /app/publish/Locales Locales/
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80