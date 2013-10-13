# Levels

1. Boost (headers only).
2. Boost DLLs: boost\_atomic.nupkg { boost\_atomic-vc110-mt-1\_54.dll, boost\_atomic-vc110-mt\_54.lib }. Ideally, Boost DLLs with the same name should be the same everywhere.
3. Boost CPPs: boost\_atomic\_cpp { boost\\atomic\\src\\lockpool.cpp }
   This is the most flexible solution for any configuration.

CPP and DLL packages should be referenced only from executable or DLLs. They must not be referenced from static libraries. This is another reason why boost header packages should be separated from CPP, LIB and DLL packages.

# Guide

1. Split C++ file set to libraries and sublibraries. 
2. ".cpp" files should provide functionality (method bodys on any platform). ".ipp" files can be responsible for platfrom specific 
   implementations.
