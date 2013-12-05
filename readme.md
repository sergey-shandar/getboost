# Levels

1. **[boost](https://www.nuget.org/packages/boost/1.55.0.10)**, headers only. For example **boost.1.54.0.0.nupkg**
2. **boost\_{library\_name}**, C++ files. For example, **boost\_mpi\_python.1.54.0.0.nupkg**.
3. **boost\_{library\_name}-{compiler}**, DLL/lib files. For example, **boost\_atomic-vc110.1.54.0.0.nupkg**.

# Parameters

* Compiler (toolset):
  * Visual C++ 2010 (vc100), 
  * Visual C++ 2012 (vc110), 
  * Visual C++ 2013 (vc120).
* Configuration:
  * Release,
  * Debug (gd).
* Address Model (address-model):
  * 32,
  * 64.
* Link: 
  * static (link=static, libboost\*.lib),
  * runtime-static (link=static runtime-link=static, libboost\*-s\*.lib),
  * shared (link=shared, boost\*.dll boost\*.lib), 

# New Version

1. **[boost](https://www.nuget.org/packages/boost/1.55.0.10)**, headers only. For example **boost.1.55.0.10.nupkg**
2. **boost\_{library\_name}**, C++ files. As a future replacement for **libboost\*** packages, like 4 and 5. For example, **boost\_mpi\_python.1.55.0.10.nupkg**.
3. **boost\_{library\_name}-{compiler}**, DLL/lib files. For example, **boost\_atomic-vc110.1.55.0.10.nupkg**.
4. **libboost\_{library\_name}-{compiler}**, lib files. For example, **libboost\atomic-vc110.1.55.0.10.nupkg**.
5. **libboost\_{library\_name}-s-{compiled}**, lib with static runtime. For example, **libboost\atomic-s-vc110.1.55.0.10.nupkg.

# Using Official Binaries

1. DLL. For example,
  boost-atomic-vc100.1.55.0.14.nupkg 
    address-model-32\boost-atomic-vc100-mt-1_55.lib 
    address-model-32\boost-atomic-vc100-mt-1_55.dll 
    address-model-32\boost-atomic-vc100-mt-gd-1_55.lib 
    address-model-32\boost-atomic-vc100-mt-gd-1_55.dll 
    address-model-64\boost-atomic-vc100-mt-1_55.lib
    address-model-64\boost-atomic-vc100-mt-1_55.dll 
    address-model-64\boost-atomic-vc100-mt-gd-1_55.lib 
    address-model-64\boost-atomic-vc100-mt-gd-1_55.dll 
  boost-wserialization-vc100.1.55.0.14.nupkg
    address-model-32\boost-wserialization-vc100-1_55.lib 
    address-model-32\boost-wserialization-vc100-1_55.dll 
    address-model-32\boost-wserialization-vc100-gd-1_55.lib 
    address-model-32\boost-wserialization-vc100-gd-1_55.dll 
    address-model-32\boost-wserialization-vc100-mt-1_55.lib 
    address-model-32\boost-wserialization-vc100-mt-1_55.dll 
    address-model-32\boost-wserialization-vc100-mt-gd-1_55.lib 
    address-model-32\boost-wserialization-vc100-mt-gd-1_55.dll
    address-model-64\boost-wserialization-vc100-1_55.lib
    address-model-64\boost-wserialization-vc100-1_55.dll 
    address-model-64\boost-wserialization-vc100-gd-1_55.lib 
    address-model-64\boost-wserialization-vc100-gd-1_55.dll 
    address-model-64\boost-wserialization-vc100-mt-1_55.lib
    address-model-64\boost-wserialization-vc100-mt-1_55.dll 
    address-model-64\boost-wserialization-vc100-mt-gd-1_55.lib 
    address-model-64\boost-wserialization-vc100-mt-gd-1_55.dll
2. LIB (thread=single?, runtime-link=shared). For example,
  libboost-wserialization-vc100.1.55.0.14.nupkg
    address-model-32\libboost-wserialization-vc100-1_55.lib
    address-model-32\libboost-wserialization-vc100-gd-1_55.lib
    address-model-64\libboost-wserialization-vc100-1_55.lib
    address-model-64\libboost-wserialization-vc100-gd-1_55.lib
3. LIB. (thread=single?, runtime-link=static). For example,
  libboost-wserialization-vc100-s.1.55.0.14.nupkg
    address-model-32\libboost-wserialization-vc100-s-1_55.lib
    address-model-32\libboost-wserialization-vc100-sgd-1_55.lib
    address-model-64\libboost-wserialization-vc100-s-1_55.lib
    address-model-64\libboost-wserialization-vc100-sgd-1_55.lib
4. LIB. (thread=multi?, runtime-link=shared). For example,
  libboost-wserialization-vc100-mt.1.55.0.14.nupkg
    address-model-32\libboost-wserialization-vc100-mt-1_55.lib
    address-model-32\libboost-wserialization-vc100-mt-gd-1_55.lib
    address-model-64\libboost-wserialization-vc100-mt-1_55.lib
    address-model-64\libboost-wserialization-vc100-mt-gd-1_55.lib
5. LIB. (thread=multi?, runtime-link=static). For example,
  libboost-wserialization-vc100-mt-s.1.55.0.14.nupkg
    address-model-32\libboost-wserialization-vc100-mt-s-1_55.lib
    address-model-32\libboost-wserialization-vc100-mt-sgd-1_55.lib
    address-model-64\libboost-wserialization-vc100-mt-s-1_55.lib
    address-model-64\libboost-wserialization-vc100-mt-sgd-1_55.lib
