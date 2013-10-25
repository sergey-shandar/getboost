# Levels

1. **boost**, headers only. For example **boost.1.54.0.0.nupkg**
2. **boost\_{library\_name}**, C++ files. For example, **boost\_mpi\_python.1.54.0.0.nupkg**.
3. **boost\_{library\_name}-{compiler}**, DLL/lib files. For example, **boost\_atomic-vc110.1.54.nupkg**.

# Packages

## boost\_atomic
- **boost\_atomic** (source)
- **boost\_atomic-vc110** (static/shared)

## boost\_chrono

The package depends on **boost\_system**.

- **boost\_chrono** (source)
- **boost\_chrono-vc110** (static/shared).

## boost\_context.
- source library needs ASM.
- **boost\_context-vc110** (static/shared)

## boost\_coroutine
The package depends on **boost\_context**.

- **boost\_coroutine** (source)
- **boost\_coroutine-vc110** (static/shared)

## boost\_date\_time
- **boost\_date\_time** (source)
- **boost\_date\_time-vc110** (static/shared)

## boost\_exception
- **boost\_exception** (source)
- **boost\_exception-vc110** (static)

## boost\_filesystem
The package depends on **boost\_system**.

- **boost\_filesystem** (source)
- **boost\_filesystem-vc110** (static/shared)

## boost\_graph
The package depends on **boost\_regex**.

- **boost\_graph** (source)
- **boost\_graph-vc110** (static/shared)
  
## boost\_graph\_parallel
The package depends on **boost\_mpi**, **boost\_serialization**, **MPI**.

- **boost\_graph** (source)

## boost\_iostreams
- **boost\_iostreams** (source)
- **boost\_iostreams-vc110** (static/shared)

## boost\_iostreams\_bzip2
The package depends on **bzip2**.

- **boost\_iostreams\_bzip2** (source).

## boost\_iostreams\_zlib
The package depends on **zlib**.

- **boost\_iostreams\_zlib** (source).

## boost\_locale

The package dpends on **boost\_thread**, **boost\_date\_time**, **boost\_system**, **boost\_chrono**.

- **boost\_locale** (source).
- **boost\_locale-vc110** (static/shared).

## boost\_locale\_icu

The package depends on **ICU**, **boost\_locale**, **boost\_thread**, **boost\_date\_time**, **boost\_system**, **boost\_chrono**.

- **boost\_locale\_icu** (source).
 
## boost\_log

The package depends on **boost\_system**, **boost\_filesystem**, **boost\_date\_time**, **boost\_thread**, **boost\_chrono**.

- **boost\_log** (source)
- **boost\_log-vc110** (static/shared)

## boost\_log\_setup

The package depends on **boost\_log\_event**, **boost\_log**, **boost\_system**, **boost\_filesystem**, **boost\_date\_time**, **boost\_thread**, **boost\_chrono**.

- source library needs a resource builder
- **boost\_log\_setup-vc110** (static/shared)

## boost\_math

- **boost\_math** (source).
- **boost\_math\_c99-vc110** (static/shared)
- **boost\_math\_c99f-vc110** (static/shared)
- **boost\_math\_c99l-vc110** (static/shared)
- **boost\_math\_tr1-vc110** (static/shared)
- **boost\_math\_tr1f-vc110** (static/shared)
- **boost\_math\_tr1l-vc110** (static/shared)

## boost\_mpi

The package depends on **MPI**, **boost\_serialization**.

- **boost\_mpi** (source).

## boost\_mpi\_python

The package depends on **MPI**, **Python**, **boost\_mpi**, **boost\_pyhton**, **boost\_serialization**.

- **boost\_mpi\_python** (source).

## boost\_program\_options
- **boost\_program\_options** (source)
- **boost\_program\_options-vc110** (static/shared)

## boost\_python

The package depends on **Python**.
- **boost\_python** (source)
  
## boost\_random
- **boost\_random** (source)
- **boost\_random-vc110** (static/shared)

## boost\_regex
- **boost\_regex** (source)
- **boost\_regex-vc110** (static/shared)

## boost\_serialization
- **boost\_serialization** (source)
- **boost\_serialization-vc110** (static/shared)
- **boost\_wserialization-vc110** (static/shared)

## boost\_signals
- **boost\_signals** (source)
- **boost\_signals-vc110** (static/shared)

## boost\_smart\_ptr
- **boost\_smart\_ptr** (source)

## boost\_system
- **boost\_system** (source)
- **boost\_system-vc110** (static/shared)

## boost\_test.
  
- **boost\_test** (source)
- **boost\_test\_cpp\_main** (source)
- **boost\_test\_test\_main** (source)
- **boost\_test\_unit\_test\_main** (source)
- **boost\_prg\_exec\_monitor-vc110**
- **boost\_unit\_test\_framework-vc110**
- **boost\_test\_exec\_monitor-vc110**

## boost\_thread 

The package depends on **boost\_date\_time**, **boost\_system**, **boost\_chrono**.

- **boost\_thread** (source)
- **boost\_thread-vc110** (static/shared)

## boost\_timer

The package depends on **boost\_system**, **boost\_chrono**.

- **boost\_timer** (source)
- **boost\_timer-vc110** (static/shared)

## boost\_wave

The package depends on **boost\_chrono**, **boost\_date\_time**, **boost\_system**, **boost\_thread**, **boost\_wave**.

- **boost\_wave** (source)
- **boost\_wave-vc110** (static/shared)

# Recommendations to Boost library authors

1. Split C++ file set to libraries and sub-libraries. 
2. **.cpp** files should provide functionality (method bodies on any platform). **.ipp** files can be responsible for platform specific implementations.