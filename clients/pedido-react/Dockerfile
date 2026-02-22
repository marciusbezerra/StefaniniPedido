FROM node:20-alpine AS build-react
WORKDIR /app
COPY clients/pedido-react/package*.json ./
RUN npm install
COPY clients/pedido-react/ .
ENV VITE_API_URL=/api
RUN npm run build

FROM nginx:alpine
COPY --from=build-react /app/dist /usr/share/nginx/html
COPY clients/pedido-react/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
