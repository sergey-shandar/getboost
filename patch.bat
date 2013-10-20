setlocal
set PATH=%PATH%;C:\Program Files (x86)\Git\cmd\
set GETBOOST=..\getboost\

cd ..\boost_1_54_0

git apply %GETBOOST%001-coroutine.patch
git apply %GETBOOST%002-date-time.patch
git apply %GETBOOST%003-log.patch
git apply %GETBOOST%004-thread.patch

# 1. change boost_context to boost_coroutine in file boost/coroutine/detail/config.hpp
# 2. replace all make_tuple calls to boost::python::make_tupel in file mpi/src/python/py_nonblocking.cpp

endlocal