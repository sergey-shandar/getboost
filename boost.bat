echo on
cd ..\boost\

call bootstrap.bat

b2 headers

rem add 'call :link XX.X' if you need to run for specific version of Visual C++ compiler.

setlocal
call "C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\SetEnv.Cmd"               
call :link 10.0
endlocal

setlocal
rem call "C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC\vcvarsall.bat"
rem call :link 14.0
endlocal

rem call :link 12.0
rem call :link 11.0

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
call :address_model %1 %2 %3 multi
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
rem change this line if you need to specify additional options to compiler.
b2 architecture=x86 link=%2 runtime-link=%3 threading=%4 address-model=%5 stage --stagedir=address-model-%5 --toolset=msvc-%1 --without-python
goto :eof
