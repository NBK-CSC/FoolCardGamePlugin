FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY . .
EXPOSE 4201
ENTRYPOINT ["dotnet", "Lib/DarkRift.Server.Console.dll"]