FROM node:20-alpine

WORKDIR /app

COPY ./../design ./design
COPY . ./web

WORKDIR /app/web

RUN npm install
RUN npm run build

EXPOSE 80

CMD ["npm", "start"]