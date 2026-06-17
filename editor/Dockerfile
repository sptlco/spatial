FROM codercom/code-server:latest

USER root

COPY ./config/code/data/ /usr/lib/code-server/

RUN curl -fsSL https://deb.nodesource.com/setup_20.x | bash - && apt-get install -y nodejs build-essential

RUN wget https://packages.microsoft.com/config/debian/13/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
	dpkg -i packages-microsoft-prod.deb && \
	rm packages-microsoft-prod.deb && \
	apt-get update && \
	apt-get install -y dotnet-sdk-10.0

USER coder

WORKDIR /home/coder/project

EXPOSE 8080
EXPOSE 3000
EXPOSE 5000
EXPOSE 9010
