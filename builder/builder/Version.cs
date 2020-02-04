using System;

namespace builder
{
    public abstract class Version
    {
        public readonly uint Major;

        public readonly uint Minor;

        public readonly uint Patch;

        public readonly uint PackageVersion;

        public abstract T Switch<T>(
            Func<StableVersion, T> stable, Func<UnstableVersion, T> unstable);

        protected Version(uint major, uint minor, uint patch, uint packageVersion)
        {
            if (packageVersion < 1)
            {
                throw new ArgumentOutOfRangeException(
                    "packageVersion",
                    "Package Version has to be greater or equal to 1");
            }

            Major = major;
            Minor = minor;
            Patch = patch;
            PackageVersion = packageVersion;
        }

        public string BaseString => Major + "." + Minor + "." + Patch + "." + PackageVersion;

        public override string ToString() => BaseString;
    }

    public sealed class UnstableVersion : Version
    {
        public readonly string PreReleaseVersion;

        public UnstableVersion(
            uint major, uint minor, uint majorRevision, uint packageVersion, string preReleaseVersion) :
            base(major, minor, majorRevision, packageVersion)
        {
            PreReleaseVersion = preReleaseVersion;
        }

        public override T Switch<T>(Func<StableVersion, T> stable, Func<UnstableVersion, T> unstable)
            => unstable(this);

        public override string ToString()
            => base.ToString() + "-" + PreReleaseVersion;
    }

    public sealed class StableVersion : Version
    {
        public StableVersion(uint major, uint minor, uint patch, uint packageVersion) :
            base(major, minor, patch, packageVersion)
        {
        }

        public override T Switch<T>(
            Func<StableVersion, T> stable, Func<UnstableVersion, T> unstable)
            => stable(this);
    }
}
