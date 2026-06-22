FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY InvoiceApp.sln ./
COPY src/InvoiceApp.Domain/InvoiceApp.Domain.csproj src/InvoiceApp.Domain/
COPY src/InvoiceApp.Application/InvoiceApp.Application.csproj src/InvoiceApp.Application/
COPY src/InvoiceApp.Infrastructure/InvoiceApp.Infrastructure.csproj src/InvoiceApp.Infrastructure/
COPY src/InvoiceApp.API/InvoiceApp.API.csproj src/InvoiceApp.API/

RUN dotnet restore src/InvoiceApp.API/InvoiceApp.API.csproj

COPY . .
WORKDIR /src/src/InvoiceApp.API
RUN dotnet publish InvoiceApp.API.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "InvoiceApp.API.dll"]