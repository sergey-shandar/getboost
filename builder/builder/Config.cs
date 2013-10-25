using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace builder
{
    static class Config
    {
        public static readonly Version Version = new Version(1, 54, 0, 156);

        public const string Authors = "Sergey Shandar, Boost";

        public const string BoostDir = @"..\..\..\..\..\boost_1_54_0\";

        public static readonly Platform[] PlatformList = new[]
        {
            new Platform("Win32", @"stage_x86\lib"),
            new Platform("x64", @"stage_x86_64\lib")
        };

        public static readonly Library[] LibraryList = new[]
        {
            // chrono depends on system
            // context needs ASM.
            // coroutine
            new Library(
                name: "coroutine",
                packageList: new[]
                {
                    // coroutine
                    new Package(),
                    // coroutine_segmented (GCC only)
                    new Package(
                        name: "segmented",
                        fileList: new[] 
                        {
                            @"detail\segmented_stack_allocator.cpp" 
                        }),
                    // coroutine_posix (need Posix library).
                    new Package(
                        name: "posix",
                        fileList: new[]
                        {
                            @"detail\standard_stack_allocator_posix.cpp",
                        }),
                }),
            // filesystem depends on system.
            // graph depens on regex.
            // graph_parallel depends on mpi.
            // iostreams
            new Library(
                name: "iostreams",
                packageList: new[]
                {
                    new Package(),
                    // need bzip2 lib.
                    new Package(
                        name: "bzip2", 
                        fileList: new[] { "bzip2.cpp" }),
                    // need zlib lib.
                    new Package(
                        name: "zlib",
                        fileList: new[] { "zlib.cpp", "gzip.cpp" }),
                }),
            // locale. WIP.
            new Library(
                name: "locale",
                packageList: new[]
                {
                    // locale, depends on thread, system, date_time, chrono.
                    new Package(
                        name: null,
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
                    new Package(
                        name: "posix",
                        fileList: new[]
                        {
                            "posix",
                        }
                    ),
                    // locale_icu
                    new Package(
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
                    new Package(
                        name: null,
                        lineList: new[] 
                        {
                            "#define BOOST_SPIRIT_USE_PHOENIX_V3"
                        },
                        fileList: new[]
                        {
                            "spirit_encoding.hpp",
                            "windows_version.hpp",
                        }),
                    new Package(
                        name: "event",
                        fileList: new[]
                        {
                            "event_log_backend.cpp"
                        }),
                    // need log_event
                    new Package(
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
                        }),
                }),
            // mpi depends on serialization and MPI (3rd party).
            new Library(
                name: "mpi",
                packageList: new[]
                {
                    // mpi
                    new Package(),
                    // mpi_python
                    new Package(
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
                    new Package(
                        name: null,
                        preprocessorDefinitions: 
                            new[] { "BOOST_PYTHON_STATIC_LIB" },
                        lineList: 
                            new[] { "#define BOOST_PYTHON_SOURCE" }
                    )
                }),
            // regex.
            // serialization.
            // test
            new Library(
                name: "test",
                packageList: new[]
                {
                    // test
                    new Package(),
                    // test_cpp_main
                    new Package(
                        name: "cpp_main",
                        fileList: new[]
                        {
                            "cpp_main.cpp"
                        }),
                    // test_test_main
                    new Package(
                        name: "test_main",
                        fileList: new[]
                        {
                            "test_main.cpp"
                        }),
                    // test_unit_test_main
                    new Package(
                        name: "unit_test_main",
                        fileList: new[]
                        {
                            "unit_test_main.cpp"
                        })
                }),
            // thread.
            new Library(
                name: "thread",
                packageList: new[]
                {
                    // thread, depends on date_time, system, chrono.
                    new Package(
                        name: null,
                        lineList: new[]
                        {
                            "#define BOOST_HAS_WINTHREADS",
                            "#define BOOST_THREAD_BUILD_LIB",
                        }),
                    // thread_pthread
                    new Package(
                        name: "pthread",
                        fileList: new[]
                        {
                            "pthread"
                        }),
                }),
        };

    }
}
