# Levels

1. **[boost](https://www.nuget.org/packages/boost/1.55.0.10)**, headers only. For example **boost.1.55.0.10.nupkg**
2. **boost\_{library\_name}**, C++ files. For example, **boost\_mpi\_python.1.55.0.10.nupkg**.
3. **boost\_{library\_name}-{compiler}**, DLL/lib files. For example, **boost\_atomic-vc110.1.55.0.10.nupkg**.

# Parameters

* Library:
  * atomic (boost_atomic\*)
  * ...
  * wave
* Sublibraries. About 20 sublibraries. For example, math:
  * math_c99 (boost_math_c99\*)
  * math_c99f
  * math_c99l
  * math_tr1
  * math_tr1f
  * math_tr1l
* Compiler (toolset), 3 compilers:
  * Visual C++ 2010 (-vc100), 
  * Visual C++ 2012 (-vc110), 
  * Visual C++ 2013 (-vc120).
* Link (link, runtime-link), 3 configurations: 
  * static (link=static, libboost\*.lib),
  * runtime-static (link=static runtime-link=static, libboost\*-s\*.lib),
  * shared (link=shared, boost\*.dll boost\*.lib),
* Threading (threading), 2 configurations:
  * single
  * multiple (-mt)
* Address Model (address-model), 2 configurations:
  * 32 (address-model-32/\*)
  * 64 (address-model-64/\*)
* Configuration, 2 configurations:
  * Release
  * Debug (gd)

About 20 * 3 * 3 * 2 * 2 * 2 files.
