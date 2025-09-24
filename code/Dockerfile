FROM mcr.microsoft.com/dotnet/sdk:9.0

RUN apt-get update && apt-get install -y \
    curl \
    sudo \
    gnupg \
    && rm -rf /var/lib/apt/lists/*

RUN curl -fsSL https://code-server.dev/install.sh | sh

ENV PROJECT_DIR=/usr/src/spatial
ENV CONFIG_DIR=/root/.config/code-server
ENV DATA_DIR=/root/.local/share/code-server

RUN mkdir -p ${PROJECT_DIR} ${CONFIG_DIR} ${DATA_DIR}

COPY entry.sh /usr/local/bin/entry.sh
RUN chmod +x /usr/local/bin/entry.sh

WORKDIR ${PROJECT_DIR}

EXPOSE 8080

ENTRYPOINT ["/usr/local/bin/entry.sh"]
