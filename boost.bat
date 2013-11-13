cd ..\boost_1_55_0\

call bootstrap.bat

setlocal
bjam msvc architecture=x86 link=static,shared stage --stagedir=stage_x86
rem bjam msvc architecture=x86 link=static,shared stage --stagedir=stage_x86 --toolset=msvc-11.0 
endlocal

setlocal
bjam msvc architecture=x86 link=static,shared address-model=64 stage --stagedir=stage_x86_64
rem bjam msvc architecture=x86 link=static,shared address-model=64 stage --stagedir=stage_x86_64 --toolset=msvc-11.0 
endlocal

setlocal
rem bjam msvc architecture=arm --without-context stage --stagedir=stage_arm
endlocal

endlocal
