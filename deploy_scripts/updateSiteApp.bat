@echo off

:: подменить физическую папку виртауальной директории связанной с приложением
::---------------------------------------------------------------------------------------
::%1 имя сайта
::%2 имя приложения и виртальной папки
::%3 новый физический путь
::---------------------------------------------------------------------------------------

set APPCMD_PATH=C:\Windows\System32\inetsrv\appcmd
set SITE_NAME=%1
set APP_NAME=%2
set physPath=%3

echo stoping site %SITE_NAME%
%APPCMD_PATH% stop site %SITE_NAME%

echo deliting app "%SITE_NAME%"/%APP_NAME%
%APPCMD_PATH% DELETE app "%SITE_NAME%"/%APP_NAME%

echo adding app "%SITE_NAME%"/%APP_NAME% with physicalPath %physPath%
%APPCMD_PATH% ADD APP /site.name:"%SITE_NAME%" /app.name:"%APP_NAME%" /path:"/%APP_NAME%" /physicalPath:%physPath%

echo restarting site
%APPCMD_PATH% start site %SITE_NAME%