cd ..\boost\

call bootstrap.bat

b2 headers

call :link msvc-12.0
rem call :link msvc-11.0

setlocal
rem call "c:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\SetEnv.Cmd"
rem call :link msvc-10.0
endlocal

goto :eof

:link
echo link {
echo toolset=%1
echo }
call :threading %1 shared shared
call :threading %1 static shared
call :threading %1 static static
goto :eof

:threading
echo threading {
echo toolset=%1
echo link=%2
echo runtime-link=%3
echo }
call :address_model %1 %2 %3 single
call :address_model %1 %2 %3 multiple
goto :eof

:address_model
echo address_model {
echo toolset=%1
echo link=%2
echo runtime-link=%3
echo threading=%4
echo }
call :build %1 %2 %3 %4 32
call :build %1 %2 %3 %4 64
goto :eof

:build
echo build {
echo toolset=%1
echo link=%2
echo runtime-link=%3
echo threading=%4
echo address-model=%5
echo }
b2 msvc architecture=x86 link=%2 runtime-link=%3 threading=%4 address-model=%5 stage --stagedir=address-model-%5 --toolset=%1 --without-python
goto :eof
