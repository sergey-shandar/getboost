# Levels

1. boost, headers only. For example boost.1.54.0.0.nupkg
2. boost_{library_name}, C++ files. For example, boost_mpi_python.1.54.0.0.nupkg.
3. boost_{library_name}-{compiler}, DLL/lib files. For example, boost_atomic-vc110.1.54.nupkg.

# Recomendations to Boost library authors

1. Split C++ file set to libraries and sublibraries. 
2. ".cpp" files should provide functionality (method bodys on any platform). ".ipp" files can be responsible for platfrom specific implementations.

# Table

<table>
  <tr>
    <td>X</td><td>Y</td>
  </tr>
</table>