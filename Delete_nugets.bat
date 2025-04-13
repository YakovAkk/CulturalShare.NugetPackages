@echo off
setlocal enabledelayedexpansion

for /r %%d in (.) do (
    if exist "%%d\bin\Debug" (
        pushd "%%d\bin\Debug" >nul 2>&1
        for %%f in (*.nupkg) do (
            echo Deleting: %%~nxf in %%d\bin\Debug
            del "%%f" >nul 2>&1
        )
        popd
    )
)