FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
EXPOSE 8080
WORKDIR /
COPY /Credito.ContraCheque.API ./app
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
WORKDIR /app
ENTRYPOINT ["dotnet", "/app/bin/Release/net8.0/Credito.ContraCheque.API.dll"]