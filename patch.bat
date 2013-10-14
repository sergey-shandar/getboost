setlocal
set PATH=%PATH%;C:\Program Files (x86)\Git\cmd\
set GETBOOST=..\getboost\

cd ..\boost_1_54_0

git apply %GETBOOST%001-coroutine.patch
git apply %GETBOOST%002-date-time.patch
git apply %GETBOOST%003-log.patch
git apply %GETBOOST%004-thread.patch

# change boost_context to boost_coroutine in file boost/coroutine/detail/config.hpp

endlocal