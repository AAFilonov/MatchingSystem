@echo off
:: поменять плейсхолдер в указанной папке на переданное значение
::-----------------------------------
::%1 путь к файлу
::%2 название файла
::%3 имя плейсхолдера
::%4 имя файла из корого берется текст для замены
::-----------------------------------

set THIS_FOLDER=C:\GitLabUtils

setlocal EnableExtensions EnableDelayedExpansion

set TARGET_PATH=%1
set TARGET_FILENAME=%2
set PLACEHOLDER_NAME=PLACEHOLDER
set REPLACEMNET_TEXT_PATH=%4
set /p REPLACEMNET_TEXT=<%REPLACEMNET_TEXT_PATH%
echo %REPLACEMNET_TEXT%

set "OUTTEXTFILE=out.tmp"

echo replacing placholder %PLACEHOLDER_NAME% in %TARGET_PATH%\%TARGET_FILENAME% to %REPLACEMNET_TEXT%
for /f "delims=" %%A in ('type "%TARGET_PATH%\%TARGET_FILENAME%"') do (
    set "string=%%A"
    set "modified=!string:PLACEHOLDER=%REPLACEMNET_TEXT%!"
    echo !modified!>>"%OUTTEXTFILE%"
)


rename "%OUTTEXTFILE%" "%TARGET_FILENAME%"
xcopy /I /Y %TARGET_FILENAME% %TARGET_PATH%\%TARGET_FILENAME%
del "%TARGET_FILENAME%"
endlocal
