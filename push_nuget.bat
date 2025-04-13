@echo off
setlocal enabledelayedexpansion

REM === NuGet settings ===
set NUGET_SOURCE=https://nuget.pkg.github.com/YakovAkk/index.json
set API_KEY=******

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
