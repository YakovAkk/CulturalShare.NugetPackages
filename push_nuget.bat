@echo off
setlocal enabledelayedexpansion

+if "%NUGET_API_KEY%"=="" (
+  echo ERROR: Please set NUGET_API_KEY environment variable.
+  exit /b 1
+)
+set API_KEY=%NUGET_API_KEY%

REM === NuGet settings ===
set NUGET_SOURCE=https://nuget.pkg.github.com/YakovAkk/index.json

REM === Loop through all folders ===
for /r %%d in (.) do (
    if exist "%%d\bin\Debug" (
        pushd "%%d\bin\Debug" >nul 2>&1
        for %%f in (*.nupkg) do (
            echo Pushing: %%~nxf from %%d\bin\Debug
            dotnet nuget push "%%f" -s %NUGET_SOURCE% --api-key %API_KEY%
        )
        popd
    )
)

echo.
echo ✅ Done pushing all .nupkg files.
