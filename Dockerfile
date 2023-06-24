# Определение базового образа
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Определение образа для сборки
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Копирование файлов проектов BLL, DAL и WEB
COPY ["BimDataControlPanel.BLL/", "BimDataControlPanel.BLL/"]
COPY ["BimDataControlPanel.DAL/", "BimDataControlPanel.DAL/"]
COPY ["BimDataControlPanel.WEB/", "BimDataControlPanel.WEB/"]

# Установка инструмента Entity Framework Core
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Возвращение в рабочую директорию для сборки веб-приложения
WORKDIR "/src/BimDataControlPanel.WEB"
RUN dotnet build -c Release -o /app/build

# Определение образа для публикации
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Определение конечного образа
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Копирование файлов из папки Dbs внутрь контейнера 
#COPY ["BimDataControlPanel.WEB/Dbs", "/app/Dbs"]


# Создание Docker Volume для папки /app/Dbs
#VOLUME /app/Dbs


# Запуск веб-приложения
ENTRYPOINT ["dotnet", "BimDataControlPanel.WEB.dll"]

# Запускать через
# docker run -d -p 3000:80 -v C:\Users\Chamion\ChamionRepo\Chamion\Web\BimDataPanel\BimDataControlPanel\BimDataControlPanel.WEB\Dbs:/app/Dbs bimdatapanel
