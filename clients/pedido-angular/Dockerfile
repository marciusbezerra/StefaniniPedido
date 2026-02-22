FROM node:20-alpine AS build-angular
WORKDIR /app
COPY clients/pedido-angular/package*.json ./
RUN npm ci
COPY clients/pedido-angular/ .
RUN npm run build -- --configuration production

FROM nginx:alpine
RUN apk update && apk upgrade && rm -rf /var/cache/apk/*
COPY --from=build-angular /app/dist/pedido-angular/browser /usr/share/nginx/html
COPY clients/pedido-angular/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
