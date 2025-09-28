FROM node:20-alpine

WORKDIR /app

COPY package*.json ./

RUN npm install
RUN npm run build

EXPOSE 80

CMD ["npm", "start"]