using System.Collections.Generic;
using System.Linq;
using builder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.G1;

namespace builderTest
{
    [TestClass]
    public class UnitTest
    {
        struct A<T>
        {
            public static implicit operator A<T>(T value)
            {
                return new A<T>();
            }
        }

        class X
        {
            public X(A<int> p = default(A<int>))
            {
            }

            /*
            public X(Optional<int> x = Optional.Absent)
            {                
            }
             * */
        }

        public static IEnumerable<SrcPackage> GetPackages()
        {
            return Enumerable.Empty<SrcPackage>();
        }

        public static void Lib(Optional.Class<IEnumerable<SrcPackage>> p)
        {
        }

        public static void R(Optional.Class<IEnumerable<int>> p)
        {
        }

        public static void M(Optional.Class<IEnumerable<string>> p)
        {
        }

        [TestMethod]
        public void VersionTestMethod()
        {
            int a = 3;
            new X(5);
            new X(a);
            // new Library(name: "x", packageList: GetPackages());
            var x = GetPackages();
            Lib(new SrcPackage[5]);
            // Lib(x);
            R(new int[5]);
            // R(Enumerable.Empty<int>());
            M(new string[7]);
            // M(Enumerable.Empty<string>());
            // Lib(Enumerable.Empty<Package>());
            Assert.AreEqual(new UnstableVersion(1, 56, 0, 2, "rc1").ToString(), "1.56.0.2-rc1");
            Assert.AreEqual(new StableVersion(1, 55, 0, 16).ToString(), "1.55.0.16");
        }
    }
}
