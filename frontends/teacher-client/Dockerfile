FROM node:lts-alpine as builder
WORKDIR /app
COPY frontends/teacher-client/package.json ./
COPY frontends/teacher-client/yarn.lock ./
RUN yarn install

COPY frontends/teacher-client ./
RUN yarn run build

FROM nginx:alpine as prod
EXPOSE 80
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=builder /app/dist /usr/share/nginx/html
CMD ["nginx", "-g", "daemon off;"]
