@echo off
setlocal

::set string={ "other": 1234, "year": 2016, "value": "str", "time": "05:01" }
::set string={"ApplicationSettings": {{ "BaseUrl": "https://matching.com", "ApplicationName": "DiplomaMatchingSystem" , "Version": "2.0.2", "UseMigrations": false }}
set THIS_FOLDER=C:\GitLabUtils

findstr "\"Version\"" %THIS_FOLDER%\release\appsettings.json >version.txt
set string=
set /p string=<version.txt

echo string %string%
rem Remove quotes
set string=%string:"=%
rem Remove braces
set "string=%string:~2,-1%"
rem Change colon+space by equal-sign
set "string=%string:: ==%"
rem Separate parts at comma into individual assignments
set "%string:, =" & set "%"


echo Version=%Version%
