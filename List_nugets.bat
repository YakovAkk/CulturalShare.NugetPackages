@echo off
setlocal enabledelayedexpansion

for /r %%d in (.) do (
    if exist "%%d\bin\Debug" (
        pushd "%%d\bin\Debug" >nul 2>&1
        for %%f in (*.nupkg) do (
            echo Found: %%~nxf in %%d\bin\Debug
        )
        popd
    )
)

pause