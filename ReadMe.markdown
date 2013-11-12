# Levels

1. **[boost](https://www.nuget.org/packages/boost/1.55.0.1)**, headers only. For example **boost.1.54.0.0.nupkg**
2. **boost\_{library\_name}**, C++ files. For example, **boost\_mpi\_python.1.54.0.0.nupkg**.
3. **boost\_{library\_name}-{compiler}**, DLL/lib files. For example, **boost\_atomic-vc110.1.54.nupkg**.

# Packages

## boost\_atomic
- **[boost\_atomic](https://www.nuget.org/packages/boost_atomic/1.55.0.1)** (source)
- **[boost\_atomic-vc110](https://www.nuget.org/packages/boost_atomic-vc110/1.55.0.1)** (static/shared)

## boost\_chrono

The package depends on **boost\_system**.

- **[boost\_chrono](https://www.nuget.org/packages/boost_chrono/1.55.0.1)** (source)
- **[boost\_chrono-vc110](https://www.nuget.org/packages/boost_chrono-vc110/1.55.0.1)** (static/shared).

## boost\_context.
- no source library.
- **[boost\_context-vc110](https://www.nuget.org/packages/boost_context-vc110/1.55.0.1)** (static/shared)

## boost\_coroutine
The package depends on **boost\_context**, **boost\_system**.

- **[boost\_coroutine](https://www.nuget.org/packages/boost_coroutine/1.55.0.1)** (source)
- **[boost\_coroutine-vc110](https://www.nuget.org/packages/boost_coroutine-vc110/1.55.0.1)** (static/shared)

## boost\_date\_time
- **[boost\\_date\\_time](https://www.nuget.org/packages/boost_date_time/1.55.0.1)** (source)
- **[boost\\_date\\_time-vc110](https://www.nuget.org/packages/boost_date_time-vc110/1.55.0.1)** (static/shared)

## boost\_exception
- **[boost\_exception](https://www.nuget.org/packages/boost_exception/1.55.0.1)** (source)
- **[boost\_exception-vc110](https://www.nuget.org/packages/boost_exception-vc110/1.55.0.1)** (static)

## boost\_filesystem
The package depends on **boost\_system**.

- **[boost\_filesystem](https://www.nuget.org/packages/boost_filesystem/1.55.0.1)** (source)
- **[boost\_filesystem-vc110](https://www.nuget.org/packages/boost_filesystem-vc110/1.55.0.1)** (static/shared)

## boost\_graph
The package depends on **boost\_regex**.

- **[boost\_graph](https://www.nuget.org/packages/boost_graph/1.55.0.1)** (source)
- **[boost\_graph-vc110](https://www.nuget.org/packages/boost_graph-vc110/1.55.0.1)** (static/shared)
  
## boost\_graph\_parallel
The package depends on **boost\_mpi**, **boost\_serialization**, **MPI**.

- **[boost\\_graph\\_parallel](https://www.nuget.org/packages/boost_graph_parallel/1.55.0.1)** (source)

## boost\_iostreams
- **[boost\_iostreams](https://www.nuget.org/packages/boost_iostreams/1.55.0.1)** (source)
- **[boost\_iostreams-vc110](https://www.nuget.org/packages/boost_iostreams-vc110/1.55.0.1)** (static/shared)

## boost\_iostreams\_bzip2
The package depends on **bzip2**.

- **[boost\\_iostreams\\_bzip2](https://www.nuget.org/packages/boost_iostreams_bzip2/1.55.0.1)** (source).

## boost\_iostreams\_zlib
The package depends on **zlib**.

- **[boost\\_iostreams\\_zlib](https://www.nuget.org/packages/boost_iostreams_zlib/1.55.0.1)** (source).

## boost\_locale

The package dpends on **boost\_thread**, **boost\_date\_time**, **boost\_system**, **boost\_chrono**.

- **[boost\_locale](https://www.nuget.org/packages/boost_locale/1.55.0.1)** (source).
- **[boost\_locale-vc110](https://www.nuget.org/packages/boost_locale-vc110/1.55.0.1)** (static/shared).

## boost\_locale\_icu

The package depends on **ICU**, **boost\_locale**, **boost\_thread**, **boost\_date\_time**, **boost\_system**, **boost\_chrono**.

- **[boost\\_locale\\_icu](https://www.nuget.org/packages/boost_locale_icu/1.55.0.1)** (source).
 
## boost\_log

The package depends on **boost\_system**, **boost\_filesystem**, **boost\_date\_time**, **boost\_thread**, **boost\_chrono**.

- **[boost\_log](https://www.nuget.org/packages/boost_log/1.55.0.1)** (source)
- **[boost\_log-vc110](https://www.nuget.org/packages/boost_log-vc110/1.55.0.1)** (static/shared)

## boost\_log\_setup

The package depends on **boost\_log\_event**, **boost\_log**, **boost\_system**, **boost\_filesystem**, **boost\_date\_time**, **boost\_thread**, **boost\_chrono**.

- no source library.
- **[boost\\_log\\_setup-vc110](https://www.nuget.org/packages/boost_log_setup-vc110/1.55.0.1)** (static/shared)

## boost\_math

- **[boost\_math](https://www.nuget.org/packages/boost_math/1.55.0.1)** (source).
- **[boost\\_math\\_c99-vc110](https://www.nuget.org/packages/boost_math_c99-vc110/1.55.0.1)** (static/shared)
- **[boost\\_math\\_c99f-vc110](https://www.nuget.org/packages/boost_math_c99f-vc110/1.55.0.1)** (static/shared)
- **[boost\\_math\\_c99l-vc110](https://www.nuget.org/packages/boost_math_c99l-vc110/1.55.0.1)** (static/shared)
- **[boost\\_math\\_tr1-vc110](https://www.nuget.org/packages/boost_math_tr1-vc110/1.55.0.1)** (static/shared)
- **[boost\\_math\\_tr1f-vc110](https://www.nuget.org/packages/boost_math_tr1f-vc110/1.55.0.1)** (static/shared)
- **[boost\\_math\\_tr1l-vc110](https://www.nuget.org/packages/boost_math_tr1l-vc110/1.55.0.1)** (static/shared)

## boost\_mpi

The package depends on **MPI**, **boost\_serialization**.

- **[boost\_mpi](https://www.nuget.org/packages/boost_mpi/1.55.0.1)** (source).

## boost\_mpi\_python

The package depends on **MPI**, **Python**, **boost\_mpi**, **boost\_pyhton**, **boost\_serialization**.

- **[boost\\_mpi\\_python](https://www.nuget.org/packages/boost_mpi_python/1.55.0.1)** (source).

## boost\_program\_options
- **[boost\\_program\\_options](https://www.nuget.org/packages/boost_program_options/1.55.0.1)** (source)
- **[boost\\_program\\_options-vc110](https://www.nuget.org/packages/boost_program_options-vc110/1.55.0.1)** (static/shared)

## boost\_python

The package depends on **Python**.

- **[boost\_python](https://www.nuget.org/packages/boost_python/1.55.0.1)** (source)
  
## boost\_random
- **[boost\_random](https://www.nuget.org/packages/boost_random/1.55.0.1)** (source)
- **[boost\_random-vc110](https://www.nuget.org/packages/boost_random-vc110/1.55.0.1)** (static/shared)

## boost\_regex
- **[boost\_regex](https://www.nuget.org/packages/boost_regex/1.55.0.1)** (source)
- **[boost\_regex-vc110](https://www.nuget.org/packages/boost_regex-vc110/1.55.0.1)** (static/shared)

## boost\_serialization
- **[boost\_serialization](https://www.nuget.org/packages/boost_serialization/1.55.0.1)** (source)
- **[boost\_serialization-vc110](https://www.nuget.org/packages/boost_serialization-vc110/1.55.0.1)** (static/shared)
- **[boost\_wserialization-vc110](https://www.nuget.org/packages/boost_wserialization-vc110/1.55.0.1)** (static/shared)

## boost\_signals
- **[boost\_signals](https://www.nuget.org/packages/boost_signals/1.55.0.1)** (source)
- **[boost\_signals-vc110](https://www.nuget.org/packages/boost_signals-vc110/1.55.0.1)** (static/shared)

## boost\_smart\_ptr
- **[boost\\_smart\\_ptr](https://www.nuget.org/packages/boost_smart_ptr/1.55.0.1)** (source)

## boost\_system
- **[boost\_system](https://www.nuget.org/packages/boost_system/1.55.0.1)** (source)
- **[boost\_system-vc110](https://www.nuget.org/packages/boost_system-vc110/1.55.0.1)** (static/shared)

## boost\_test.
  
- **boost\_test** (source), skipped.
- **boost\_test\_cpp\_main** (source), skipped.
- **boost\_test\_test\_main** (source), skipped.
- **boost\_test\_unit\_test\_main** (source), skipped.
- **[boost\\_prg\\_exec\\_monitor-vc110](https://www.nuget.org/packages/boost_prg_exec_monitor-vc110/1.55.0.1)**
- **[boost\\_unit\\_test\\_framework-vc110](https://www.nuget.org/packages/boost_unit_test_framework-vc110/1.55.0.1)**
- **[boost\\_test\\_exec\\_monitor-vc110](https://www.nuget.org/packages/boost_test_exec_monitor-vc110/1.55.0.1)**

## boost\_thread 

The package depends on **boost\_date\_time**, **boost\_system**, **boost\_chrono**.

- **[boost\_thread](https://www.nuget.org/packages/boost_thread/1.55.0.1)** (source)
- **[boost\_thread-vc110](https://www.nuget.org/packages/boost_thread-vc110/1.55.0.1)** (static/shared)

## boost\_timer

The package depends on **boost\_system**, **boost\_chrono**.

- **[boost\_timer](https://www.nuget.org/packages/boost_timer/1.55.0.1)** (source)
- **[boost\_timer-vc110](https://www.nuget.org/packages/boost_timer-vc110/1.55.0.1)** (static/shared)

## boost\_wave

The package depends on **boost\_chrono**, **boost\_date\_time**, **boost\_system**, **boost\_thread**, **boost\_wave**.

- **[boost\_wave](https://www.nuget.org/packages/boost_wave/1.55.0.1)** (source)
- **[boost\_wave-vc110](https://www.nuget.org/packages/boost_wave-vc110/1.55.0.1)** (static/shared)

# Recommendations to Boost library authors

1. Split C++ file set to libraries and sub-libraries. 
2. **.cpp** files should provide functionality (method bodies on any platform). **.ipp** files can be responsible for platform specific implementations.