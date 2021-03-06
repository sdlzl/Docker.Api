﻿# 1.指定编译和发布应用的镜像
FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build-env

# 2. 指定（编译和发布）工作目录
WORKDIR /app

# 3. 拷贝.csproj到工作目录/app，然后执行dotnet restore恢复所有安装的NuGet包
COPY *.csproj ./
RUN dotnet restore

# 4. 拷贝当前项目目录下所有文件到工作目录(/app)，然后执行dotnet publish命令将应用发布到/app/out目录下
COPY . ./
RUN dotnet publish -c Release -o out

# 5. 编译生成Docker镜像
# 5.1.设置基础镜像
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime

# 5.2. 设置（运行）工作目录，并将发布文件拷贝到out子目录下
WORKDIR /app
COPY --from=build-env /app/out .

# 5.3. 利用环境变量设置ASP.NET Core应用的监听地址
ENV ASPNETCORE_URLS http://0.0.0.0:3827

# 5.4. 执行dotnet命令启动ASP.NET Core应用
ENTRYPOINT ["dotnet", "Api.dll"]