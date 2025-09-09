@echo off
:: 检查项目文件
if not exist "HtmlPaperManager.csproj" (
    echo Error: HtmlPaperManager.csproj not found
    exit /b 1
)

:: 清理旧文件
if exist "bin\Release\net8.0-windows\win-x64\publish" rmdir /s /q "bin\Release\net8.0-windows\win-x64\publish" >nul 2>&1
if exist "html-paper-manager.zip" del "html-paper-manager.zip" >nul 2>&1

:: 执行发布
dotnet publish -c Release -r win-x64 --self-contained false -p:PublishSingleFile=false >nul 2>&1
if not exist "bin\Release\net8.0-windows\win-x64\publish" (
    echo Error: dotnet publish failed
    exit /b 1
)

:: 压缩文件
powershell -Command "Compress-Archive -Path 'bin\Release\net8.0-windows\win-x64\publish\*' -DestinationPath 'html-paper-manager.zip' -Force" >nul 2>&1
if not exist "html-paper-manager.zip" (
    echo Error: compression failed
    exit /b 1
)

:: 输出成功结果
echo %cd%\html-paper-manager.zip
