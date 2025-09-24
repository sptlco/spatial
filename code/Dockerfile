FROM mcr.microsoft.com/dotnet/sdk:9.0

RUN apt-get update && apt-get install -y \
    curl \
    sudo \
    gnupg \
    && rm -rf /var/lib/apt/lists/*

RUN curl -fsSL https://code-server.dev/install.sh | sh

ARG USER=spatial
ARG UID=1000
ARG GID=1000
RUN groupadd -g ${GID} ${USER} \
    && useradd -m -u ${UID} -g ${GID} -s /bin/bash ${USER} \
    && echo "${USER} ALL=(ALL) NOPASSWD:ALL" >> /etc/sudoers

ENV HOME=/home/${USER}
ENV PROJECT_DIR=${HOME}/project
ENV CONFIG_DIR=${HOME}/.config/code-server
ENV DATA_DIR=${HOME}/.local/share/code-server

RUN mkdir -p ${PROJECT_DIR} ${CONFIG_DIR} ${DATA_DIR}

COPY entry.sh /usr/local/bin/entry.sh
RUN chmod +x /usr/local/bin/entry.sh

USER ${USER}
WORKDIR ${PROJECT_DIR}

EXPOSE 8080

ENTRYPOINT ["/usr/local/bin/entry.sh"]