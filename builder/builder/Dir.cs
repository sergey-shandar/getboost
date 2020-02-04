using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Framework.G1;

namespace builder
{
    partial class Program
    {
        class Dir
        {
            private readonly DirectoryInfo Info;
            private readonly string Name;

            public Dir(DirectoryInfo info, string name)
            {
                Info = info;
                Name = name;
            }

            public IEnumerable<string> FileList(Func<string, bool> filter)
                => Info.
                    GetDirectories().
                    Select(
                        i => new Dir(i, Path.Combine(Name, i.Name))
                    ).
                    SelectMany(
                        dir =>
                            filter(dir.Name) ?
                                dir.FileList() :
                                dir.FileList(filter)
                    ).
                    Concat(
                        Info.
                        GetFiles().
                        Select(f => Path.Combine(Name, f.Name)).
                        Where(f => filter(f))
                    );

            public IEnumerable<string> FileList(IEnumerable<string> filter)
            {
                var set = filter.ToHashSet();
                return FileList(name => set.Contains(name));
            }

            public IEnumerable<string> FileList()
                => FileList(name => true);
        }

    }
}
