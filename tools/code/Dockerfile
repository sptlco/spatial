FROM mcr.microsoft.com/dotnet/sdk:9.0

# Install Node
RUN apt-get update && apt-get install -y curl sudo gnupg build-essential \
    && curl -fsSL https://deb.nodesource.com/setup_20.x | bash - \
    && apt-get install -y nodejs \
    && rm -rf /var/lib/apt/lists/*

# Install code-server
RUN curl -fsSL https://code-server.dev/install.sh | sh

# Setup workspace
WORKDIR /usr/src/spatial
COPY entry.sh /usr/local/bin/entry.sh
RUN chmod +x /usr/local/bin/entry.sh

EXPOSE 80

ENTRYPOINT ["/usr/local/bin/entry.sh"]