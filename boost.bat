@echo off

setlocal
set toolset=%1

if "%toolset%" == "" goto :no_toolset
if not exist bootstrap.bat goto :call_in_boost_directory

call bootstrap.bat

b2 headers

call :link %toolset%

goto :end

:link
echo link {
echo toolset=%1
echo }
call :threading %1 "shared,static" "shared,static"
exit /B 0

:threading
echo threading {
echo toolset=%1
echo link=%~2
echo runtime-link=%~3
echo }
call :address_model %1 %2 %3 "single,multi"
exit /B 0

:address_model
echo address_model {
echo toolset=%~1
echo link=%~2
echo runtime-link=%~3
echo threading=%~4
echo }
call :build %1 %2 %3 %4 32
call :build %1 %2 %3 %4 64
exit /B 0

:build
echo build {
echo toolset=%1
echo link=%~2
echo runtime-link=%~3
echo threading=%~4
echo address-model=%5
echo }

b2 architecture=x86 cxxstd=17 link=%~2 runtime-link=%~3 threading=%~4 address-model=%5 stage --stagedir="stage/%1/address-model-%5" --toolset=%1 --without-python

exit /B 0

:no_toolset
echo No toolset!
echo See msvc versions supported by bbv2:
echo https://boostorg.github.io/build/manual/develop/index.html#bbv2.reference.tools.compiler.msvc
exit /B 1

:call_in_boost_directory
echo It's required to call the script in boost directory
exit /B 1

:end
