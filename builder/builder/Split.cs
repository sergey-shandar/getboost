namespace builder
{
    sealed class Split
    {
        private readonly string Source;

        private readonly int Index;

        public string Before
        {
            get
            {
                return Index == -1 ? Source: Source.Substring(0, Index);
            }
        }

        public string After
        {
            get
            {
                return Index == -1 ? string.Empty: Source.Substring(Index + 1);
            }
        }

        public Split(string source, int index)
        {
            Source = source;
            Index = index;
        }
    }
}
