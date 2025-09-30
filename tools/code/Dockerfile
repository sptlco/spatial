FROM mcr.microsoft.com/dotnet/sdk:9.0

RUN apt-get update && apt-get install -y curl sudo gnupg build-essential \
    && curl -fsSL https://deb.nodesource.com/setup_20.x | bash - \
    && apt-get install -y nodejs \
    && rm -rf /var/lib/apt/lists/*

RUN curl -fsSL https://code-server.dev/install.sh | sh

WORKDIR /usr/src/spatial
COPY entry.sh /usr/local/bin/entry.sh
RUN chmod +x /usr/local/bin/entry.sh

RUN cd api
RUN npm install
RUN cd ..

RUN cd cloud
RUN dotnet restore
RUN cd ..

RUN cd interface/mobile
RUN npm install
RUN cd ../..

RUN cd interface/web
RUN npm install
RUN cd ../..

EXPOSE 80

ENTRYPOINT ["/usr/local/bin/entry.sh"]