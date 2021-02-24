namespace FormatFinderCore
{
    public struct FormatInfo
    {
        public PaperFormatName Name { get; private set; }
        public bool IsStandard { get; private set; }

        public FormatInfo(PaperFormatName name, bool isStandart)
        {
            Name = name;
            IsStandard = isStandart;
        }
    }
}
