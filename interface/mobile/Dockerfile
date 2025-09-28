FROM node:20-bullseye-slim

ARG NODE_ENV=production
ENV NODE_ENV $NODE_ENV

ARG PORT=19006
ENV PORT $PORT
EXPOSE $PORT 19000 19001 19002

ENV NPM_CONFIG_PREFIX=/home/node/.npm-global
ENV PATH /home/node/.npm-global/bin:$PATH
RUN npm i --unsafe-perm --allow-root -g npm@latest expo-cli@latest

RUN mkdir /opt/spatial
WORKDIR /opt/spatial
ENV PATH /opt/spatial/.bin:$PATH
COPY ./package.json ./package-lock.json ./
RUN npm install

WORKDIR /opt/spatial/app

COPY . .

ENTRYPOINT ["npm", "run"]
CMD ["dev"]
