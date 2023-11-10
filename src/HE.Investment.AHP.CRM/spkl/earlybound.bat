@echo off
set package_root=%userprofile%\.nuget\packages\spkl\1.0.640\
REM Find the spkl in the package folder (irrespective of version)
For /R %package_root% %%G IN (spkl.exe) do (
    IF EXIST "%%G" (set spkl_path=%%G
    goto :continue)
    )

:continue
@echo Using '%spkl_path%' 

cd %cd%\..

REM spkl earlybound [path] [connection-string] [/p:release]
"%spkl_path%" earlybound  %*

if errorlevel 1 (
echo Error Code=%errorlevel%
exit /b %errorlevel%
)

pause