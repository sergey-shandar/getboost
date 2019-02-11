using builder.MarkDown;
using System.Collections.Generic;

namespace builder
{
    public static class Config
    {
        public static readonly Version Version = 
            new StableVersion(1, 69, 0, 0);

        public static readonly List[] Release =
        {
        };

        public const string Authors = "Sergey Shandar, Boost";

        public const string Owners = "sergey_shandar";

        public const string BoostDir = @"..\..\..\..\..\boost\";

        public sealed class CompilerInfo
        {
            public readonly string Name;

            public readonly string PreRelease;

            public CompilerInfo(string name, string preRelease = "")
            {
                Name = name;
                PreRelease = preRelease;
            }
        }

        public static readonly Dictionary<string, CompilerInfo> CompilerMap = 
            new Dictionary<string, CompilerInfo>
        {
            { "vc80", new CompilerInfo("Visual Studio 2005 SP1") },
            { "vc90", new CompilerInfo("Visual Studio 2008 SP1") },
            { "vc100", new CompilerInfo("Visual Studio 2010 SP1") },
            { "vc110", new CompilerInfo("Visual Studio 2012 Update 4") },
            { "vc120", new CompilerInfo("Visual Studio 2013 Update 5") },
            { "vc140", new CompilerInfo("Visual Studio 2015 Update 3") },
            { "vc141", new CompilerInfo("Visual Studio 2017 15.7.4") }
        };

        public static int CompilerNumber(string key)
            => int.Parse(key.Substring(2));
            
        public static readonly Library[] LibraryList =
        {
            // chrono depends on system
            // context needs ASM.
            new Library(
                name: "context",
                packageList: new[]
                {
                    new SrcPackage(
                        skip: true
                    )
                }
            ),
            // coroutine
            new Library(
                name: "coroutine",
                packageList: new[]
                {
                    // coroutine
                    new SrcPackage(
                        preprocessorDefinitions: new[] 
                        {
                            "BOOST_COROUTINES_NO_LIB"
                        }
                    ),
                    // coroutine_segmented (GCC only)
                    new SrcPackage(
                        name: "segmented",
                        fileList: new[] 
                        {
                            @"detail\segmented_stack_allocator.cpp" 
                        },
                        skip: true
                    ),
                    // coroutine_posix (need Posix library).
                    new SrcPackage(
                        name: "posix",
                        fileList: new[]
                        {
                            @"detail\standard_stack_allocator_posix.cpp",
                        },
                        skip: true
                    ),
                }),
            // filesystem depends on system.
            // graph depens on regex.
            // graph_parallel depends on mpi.
            // iostreams
            new Library(
                name: "iostreams",
                packageList: new[]
                {
                    new SrcPackage(),
                    // need bzip2 lib.
                    new SrcPackage(
                        name: "bzip2", 
                        fileList: new[] { "bzip2.cpp" }),
                    // need zlib lib.
                    new SrcPackage(
                        name: "zlib",
                        fileList: new[] { "zlib.cpp", "gzip.cpp" }),
                }),
            // locale. WIP.
            new Library(
                name: "locale",
                packageList: new[]
                {
                    // locale, depends on thread, system, date_time, chrono.
                    new SrcPackage(
                        lineList: new[] 
                        { 
                            "#define BOOST_LOCALE_NO_POSIX_BACKEND" 
                        },
                        fileList: new[]
                        {
                            @"util\locale_data.hpp",
                            @"shared\mo_hash.hpp",
                            @"encoding\conv.hpp"
                        }
                    ),
                    // locale_posix
                    new SrcPackage(
                        name: "posix",
                        fileList: new[]
                        {
                            "posix",
                        },
                        skip: true
                    ),
                    // locale_icu
                    new SrcPackage(
                        name: "icu",
                        fileList: new[]
                        {
                            "icu",
                            @"util\locale_data.hpp",
                            @"shared\mo_hash.hpp",
                            @"encoding\conv.hpp"
                        }
                    ),
                }),
            // log
            new Library(
                name: "log",
                packageList: new[]
                {
                    // log depends on system, file_system, date_time, thread, chrono.
                    new SrcPackage(
                        lineList: new[] 
                        {
                            "#define BOOST_SPIRIT_USE_PHOENIX_V3"
                        },
                        fileList: new[]
                        {
                            "spirit_encoding.hpp",
                            "windows_version.hpp",
                        }),
                    new SrcPackage(
                        name: "event",
                        fileList: new[]
                        {
                            "event_log_backend.cpp"
                        },
                        skip: true
                    ),
                    // need log_event
                    new SrcPackage(
                        name: "setup",
                        lineList: new[]
                        {
                            "#define BOOST_SPIRIT_USE_PHOENIX_V3"
                        },
                        fileList: new[]
                        {
                            "default_filter_factory.hpp",
                            "windows_version.hpp",
                            "spirit_encoding.hpp",
                            "parser_utils.hpp",
                            "parser_utils.cpp",
                            "init_from_stream.cpp",
                            "init_from_settings.cpp",
                            "settings_parser.cpp",
                            "filter_parser.cpp",
                            "formatter_parser.cpp",
                            "default_filter_factory.cpp",
                            "matches_relation_factory.cpp",
                            "default_formatter_factory.cpp"
                        },
                        skip: true
                    ),
                }),
            // mpi depends on serialization and MPI (3rd party).
            new Library(
                name: "mpi",
                packageList: new[]
                {
                    // mpi
                    new SrcPackage(),
                    // mpi_python
                    new SrcPackage(
                        name: "python",
                        fileList: new[]
                        {
                            "python"
                        }),
                }),
            // python
            new Library(
                name: "python",
                packageList: new[]
                {
                    new SrcPackage(
                        preprocessorDefinitions: 
                            new[] { "BOOST_PYTHON_STATIC_LIB" },
                        lineList: 
                            new[] { "#define BOOST_PYTHON_SOURCE" }
                    )
                }
            ),
            // regex.
            // serialization.
            // test
            new Library(
                name: "test",
                packageList: new[]
                {
                    // test
                    new SrcPackage(
                        skip: true
                    ),
                    // test_cpp_main
                    new SrcPackage(
                        name: "cpp_main",
                        fileList: new[]
                        {
                            "cpp_main.cpp"
                        },
                        skip: true
                    ),
                    // test_test_main
                    new SrcPackage(
                        name: "test_main",
                        fileList: new[]
                        {
                            "test_main.cpp"
                        },
                        skip: true
                    ),
                    // test_unit_test_main
                    new SrcPackage(
                        name: "unit_test_main",
                        fileList: new[]
                        {
                            "unit_test_main.cpp"
                        },
                        skip: true
                    )
                }
            ),
            // thread.
            new Library(
                name: "thread",
                packageList: new[]
                {
                    // thread, depends on date_time, system, chrono.
                    new SrcPackage(
                        lineList: new[]
                        {
                            "#define BOOST_HAS_WINTHREADS",
                            "#define BOOST_THREAD_BUILD_LIB",
                        }
                    ),
                    // thread_pthread
                    new SrcPackage(
                        name: "pthread",
                        fileList: new[]
                        {
                            "pthread"
                        },
                        skip: true
                    ),
                }
            ),
        };

    }
}
