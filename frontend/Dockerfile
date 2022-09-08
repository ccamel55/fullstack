FROM node:18-alpine 
WORKDIR /frontend
ENV PATH="./node_modules/.bin:$PATH"
COPY .  .
RUN npm ci 
RUN npm run build
EXPOSE 3000
CMD ["npm", "start"]