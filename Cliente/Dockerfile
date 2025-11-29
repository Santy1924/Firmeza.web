# Production stage only
FROM node:18-alpine

WORKDIR /app

# Install serve to run the application
RUN npm install -g serve

# Copy pre-built dist from build output
COPY dist ./dist

# Expose port
EXPOSE 5173

# Start the application
CMD ["serve", "-s", "dist", "-l", "5173"]


