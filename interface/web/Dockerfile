FROM node:20-alpine

WORKDIR /app

COPY . .

RUN npm install --workspace=@spatial/web
RUN npm run build:web

WORKDIR /app/web

EXPOSE 80

CMD ["npm", "start"]
