@echo off
set DEPLOY_DIRECTORY=C:\WebFarm\projects.iipo.tu-bryansk.ru\diplom-matching-releases
set RELEASE_DIRECTORY=C:\GitLabUtils\release
set VERSIONS_STORED=8
set THIS_FOLDER=C:\GitLabUtils

::��� 1
::��������� ����� ������ �� ������ ������� ���� �� ��� �������� ����� VERSIONS_STORED
call %THIS_FOLDER%\cleanDeployDirectory.bat %DEPLOY_DIRECTORY% %VERSIONS_STORED%
echo[

::��� 2
::��������������� �������� ����� ����� - ������� ����� � ������� ������ ������ �������� ����� ������
:: ����� �������� ������� ����

::����� ����� ������ � ������ ������� ������
findstr "\"Version\""  %THIS_FOLDER%\release\\appsettings.json >version.txt
set string=
set /p string=<version.txt
del version.txt

:: ���������� ������
rem Remove quotes
set string=%string:"=%
rem Remove braces
set "string=%string:~2,-1%"
rem Change colon+space by equal-sign
set "string=%string:: ==%"
rem Separate parts at comma into individual assignments
set "%string:, =" & set "%"
echo version to deploy is %Version%
echo[

:: �������� ������� ���� �����
set mydate=%date%
For /f "tokens=1-2 delims=/:" %%a in ('time /t') do (set mytime=%%a_%%b)
echo deploy datetime is %mydate%_%mytime%
set destDirName=%Version%_%date%_%mytime%
echo[

::��� 3
:: ����������� ����� � ����� ������
FOR /F "delims=" %%i IN ('dir "%DIRECTORY%" /b /ad-h /t:c /od') DO SET MOST_RECENT_FOLDER=%%i
echo most recent dir is %MOST_RECENT_FOLDER%

echo copying %RELEASE_DIRECTORY% to %DEPLOY_DIRECTORY%\%destDirName%
xcopy /I /Y /E %DEPLOY_DIRECTORY%\%MOST_RECENT_FOLDER% %DEPLOY_DIRECTORY%\%destDirName% > nul

:: ����������� ����� � ����� ������
xcopy /I /Y /E %RELEASE_DIRECTORY% %DEPLOY_DIRECTORY%\%destDirName% > nul
if %errorlevel%==4 goto :eof
echo copying coplite
echo[

::��� 4
::�������� ����������� � ������� �� ������ �����������
set TARGET_PATH=%DEPLOY_DIRECTORY%\%destDirName%
set TARGET_FILENAME=appsettings.Production.json

call  %THIS_FOLDER%\replacePlaceholder.bat %TARGET_PATH% %TARGET_FILENAME% PLACEHOLDER %THIS_FOLDER%\connectionString.txt
echo[

::��� 5
:: ��������� ���������� ����� ������������ ���������� ��������� � ����������� diplom-matching
set SITE_NAME=projects.iipo.tu-bryansk.ru
set APP_NAME=diplom-matching
set phys_path=%DEPLOY_DIRECTORY%\%destDirName%

call  %THIS_FOLDER%\updateSiteApp.bat %SITE_NAME% %APP_NAME% %phys_path%

