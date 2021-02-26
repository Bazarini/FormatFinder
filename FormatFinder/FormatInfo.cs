using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace FormatFinderCore
{
    [Serializable]
    [XmlRoot]
    [XmlInclude(typeof(PaperFormatName))]
    public struct FormatInfo : ISerializable
    {
        [XmlAttribute("Name")]
        public PaperFormatName Name { get; set; }
        [XmlAttribute("IsStandard")]
        public bool IsStandard { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name, typeof(PaperFormatName));
            info.AddValue("IsStandard", IsStandard, typeof(bool));
        }
    }
}
