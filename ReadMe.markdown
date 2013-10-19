# Levels

1. boost, headers only. For example boost.1.54.0.0.nupkg
2. boost_{library_name}, C++ files. For example, boost_mpi_python.1.54.0.0.nupkg.
3. boost_{library_name}-{compiler}, DLL/lib files. For example, boost_atomic-vc110.1.54.nupkg.

# Recomendations to Boost library authors

1. Split C++ file set to libraries and sublibraries. 
2. ".cpp" files should provide functionality (method bodys on any platform). ".ipp" files can be responsible for platfrom specific implementations.

# Report

- boost_context - no source library.
- boost_coroutine - depends on boost_context.
- boost_graph_parallel - depends on boost_mpi.
- boost_mpi - depends on MPI library.
- boost_iostreams_bzip2 - depends on bzip2.
- boost_iostreams_zlib - depends on zlib.
- boost_locale - dpends on boost_thread, boost_date_time.

- boost_thread - depends on boost_date_time, boost_system, boost_chrono.