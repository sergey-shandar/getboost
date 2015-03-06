namespace builder
{
    public sealed class Platform
    {
        public readonly string Name;

        public readonly string Directory;

        public Platform(string name, string directory)
        {
            Name = name;
            Directory = directory;
        }

    }
}
