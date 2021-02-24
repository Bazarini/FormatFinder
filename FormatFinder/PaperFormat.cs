using System.Drawing;
using System.Runtime.Serialization;

namespace FormatFinderCore
{
    struct PaperFormat : ISerializable
    {

        public PaperFormatName Name { get; set; }
        public long MaxVal { get => _maxSize.Width; }
        Size _minSize;
        Size _maxSize;

        public PaperFormat(PaperFormatName name, Size minSize, Size maxSize) : this()
        {
            Name = name;
            if (minSize.Width < maxSize.Width)
            {
                MinSize = minSize;
                MaxSize = maxSize;
            }
            else
            {
                MinSize = maxSize;
                MaxSize = minSize;
            }
        }

        Size MinSize { get => _minSize; set => _minSize = NormalizeSize(value); }
        Size MaxSize
        {
            get => _maxSize; set => _maxSize = NormalizeSize(value);
        }
        public bool IsInRange(Size size)
        {
            size = NormalizeSize(size);
            if (size.Width >= MinSize.Width && size.Height >= MinSize.Height &&
                size.Width < MaxSize.Width && size.Height < MaxSize.Height)
                return true;
            return false;
        }
        public bool IsInParticularRange(Size size)
        {
            size = NormalizeSize(size);
            if ((size.Width >= MinSize.Width && size.Width < MaxSize.Width) ||
                (size.Height >= MinSize.Height && size.Height < MaxSize.Height))
                return true;
            return false;
        }
        private Size NormalizeSize(Size size)
        {
            if (size.Width > size.Height)
                size = new Size(size.Height, size.Width);
            return size;
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name, typeof(PaperFormatName));
            info.AddValue("MinSize", _minSize, typeof(Size));
            info.AddValue("MaxSize", _maxSize, typeof(Size));
        }
    }
}
