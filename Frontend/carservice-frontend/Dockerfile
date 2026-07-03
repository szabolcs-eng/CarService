# 1. Építési fázis (Node.js)
FROM node:20-alpine AS build
WORKDIR /app

# Függőségek telepítése
COPY package.json package-lock.json* ./
RUN npm install

# Kód másolása és a statikus fájlok (HTML/JS/CSS) legenerálása
COPY . .
RUN npm run build

# 2. Futtatási fázis (Nginx webszerver)
FROM nginx:alpine
# A Vite a /dist mappába fordít, ezt másoljuk be az Nginx-be
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]