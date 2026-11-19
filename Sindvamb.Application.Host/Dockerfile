# Executar na pasta root do projeto:

# docker build . -t sindvamb-application-host:latest  --no-cache -f .\Sindvamb.Application.Host\Dockerfile 
# docker run -p 8081:443 sindvamb-application-host:latest

# ou

# docker-compose -f .\docker-compose-api.yml up -d --build --force-recreate
 
# continuação 

# docker login registry.sindvamb.com.br -u sindvamb -p 1q2w3E*@
# docker tag sindvamb-application-host:latest registry.sindvamb.com.br/sindvamb-application-host:latest
# docker push registry.sindvamb.com.br/sindvamb-application-host:latest

# docker-compose down

# docker manifest inspect registry.sindvamb.com.br/sindvamb-application-host:latest
# docker exec registry /bin/registry garbage-collect   /etc/docker/registry/config.yml --delete-untagged
# curl -u sindvamb:1q2w3E*@ -sS -X DELETE https://registry.sindvamb.com.br/v2/sindvamb-application-host/manifests/sha256:a8e2f78ab30dbe82aba926798b38ebbad7cb0afb1480b5aa0dc2427e5b7258ba
# docker exec registry bin/registry garbage-collect --dry-run /etc/docker/registry/config.yml
# curl -u sindvamb:1q2w3E*@ -sS https://registry.sindvamb.com.br/v2/sindvamb-application-host/tags/list
# curl -u sindvamb:1q2w3E*@ -sS -X DELETE https://registry.sindvamb.com.br/v2/sindvamb-application-host/manifests/952d4cd4416ddbab5ed4bfaca7111a2ad2f40659738d12c6f37ff1a920c9094a



# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:9.0-noble AS base
USER $APP_UID
WORKDIR /app


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:9.0-noble AS build
ARG BUILD_CONFIGURATION=Release
#ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Sindvamb.Application.Host/", "Sindvamb.Application.Host/"]
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
RUN dotnet restore "./Sindvamb.Application.Host/Sindvamb.Application.Host.csproj"
COPY . .

WORKDIR "/src/Sindvamb.Application.Host"
RUN dotnet build "./Sindvamb.Application.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
#ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Sindvamb.Application.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore /p:UseAppHost=false

RUN rm -R wwwroot
#RUN rm -R tmp

RUN mkdir files
RUN mkdir wwwroot

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app


COPY  --chmod=0777 --from=publish /app/publish .
COPY --chmod=0777 ["Sindvamb.Templates/wwwroot/", "wwwroot/"]

USER root

RUN mkdir tmp

RUN chown -R $APP_UID:$APP_UID /app
RUN chown -R $APP_UID:$APP_UID /app/tmp
RUN chown -R $APP_UID:$APP_UID /tmp

RUN chmod 777 -R /app
RUN chmod 777 -R /app/tmp
RUN chmod 777 -R /tmp


RUN apt-get update -yq
RUN apt-get install -y --no-install-recommends --allow-unauthenticated libgdiplus zlib1g fontconfig libfreetype6 libx11-6 libxext6 libxrender1 wget gdebi libc6-dev libx11-dev && rm -rf /var/lib/apt/lists/*
RUN wget https://github.com/wkhtmltopdf/wkhtmltopdf/releases/download/0.12.5/wkhtmltox_0.12.5-1.stretch_amd64.deb
RUN wget http://ftp.br.debian.org/debian/pool/main/libj/libjpeg-turbo/libjpeg62-turbo_2.1.5-3.1_amd64.deb
RUN wget https://archive.ubuntu.com/ubuntu/pool/main/o/openssl/libssl1.1_1.1.0g-2ubuntu4_amd64.deb
RUN gdebi --n *.deb
RUN ln -s /usr/lib/libgdiplus.so /lib/x86_64-linux-gnu/libgdiplus.so
RUN ln -s /usr/local/lib/libwkhtmltox.so /usr/lib/libwkhtmltox.so

USER $APP_UID

EXPOSE 8080
EXPOSE 8081
EXPOSE 443/tcp
EXPOSE 465/tcp
EXPOSE 587/tcp
EXPOSE 25/tcp
EXPOSE 2525/tcp

CMD ["dotnet", "Sindvamb.Application.Host.dll"]