# Runtime stage only
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

# Copy pre-published Web.Api from publish_output
COPY publish_output .

EXPOSE 5272

ENV ASPNETCORE_URLS=http://+:5272
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "Web.Api.dll"]



