FROM node:20-alpine

WORKDIR /app

COPY . .

RUN npm install --workspace=web

WORKDIR /app/web

RUN npm run build

EXPOSE 80

CMD ["npm", "start"]
