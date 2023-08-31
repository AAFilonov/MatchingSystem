@echo off

:: проверить количество папок в директории %1, если их больше чем %2 о удалить самую старую
::---------------------------------------------------------------------------------------
::%1 директория
::%2 количество папок
::---------------------------------------------------------------------------------------
set DIRECTORY=%1
set VERSIONS_STORED=%2

::найти самую новую и самую старую папку из имеющихся
FOR /F "delims=" %%i IN ('dir "%DIRECTORY%" /b /ad-h /t:c /od') DO SET MOST_RECENT_FOLDER=%%i
echo most recent dir is %MOST_RECENT_FOLDER%
FOR /F "delims=" %%i IN ('dir "%DIRECTORY%" /b /ad-h /t:c /o-d') DO SET MOST_OLD_FOLDER=%%i
echo most old dir is %MOST_OLD_FOLDER%

set cnt=0
FOR /F "delims=" %%i IN ('dir "%DIRECTORY%" /b /ad-h /t:c /o-d') do set /a cnt+=1
echo Directories count = %cnt%

::если количество папок VERSIONS_STORED, удалить самую старую
if %cnt% GEQ %VERSIONS_STORED% (echo deleting %DIRECTORY%\%MOST_OLD_FOLDER%
rmdir /S /Q %DIRECTORY%\%MOST_OLD_FOLDER%
echo deleted) else echo no cleaning required