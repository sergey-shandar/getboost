setlocal
set PATH=%PATH%;C:\Program Files (x86)\Git\cmd\
set GETBOOST=..\getboost\

cd ..\boost_1_55_0

# 1. delete libs/mpi/src/python/exception.cpp
# 2. add "#include <algorithm>" after "#include <boost/iterator/iterator_traits.hpp>" in file boost\archive\iterators\transform_width.hpp

endlocal