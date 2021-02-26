using FormatFinderCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace FormatFinderCore
{
    [Serializable]
    [XmlInclude(typeof(PagesWithFormat))]
    [XmlInclude(typeof(FormatInfo))]
    [XmlInclude(typeof(PaperFormatName))]
    public class PagesWithFormatCollection : ISerializable, ICollection<PagesWithFormat>, IList<PagesWithFormat>
    {
        public PagesWithFormatCollection()
        {
            PagesWithFormat = new List<PagesWithFormat>();
        }
        [XmlElement("PagesWithFormatList")]
        [XmlArray("PagesWithFormatList")]
        public List<PagesWithFormat> PagesWithFormat { get; set; }

        #region ICollection
        public object this[int index] { get => ((IList)PagesWithFormat)[index]; set => ((IList)PagesWithFormat)[index] = value; }
        PagesWithFormat IList<PagesWithFormat>.this[int index] { get => ((IList<PagesWithFormat>)PagesWithFormat)[index]; set => ((IList<PagesWithFormat>)PagesWithFormat)[index] = value; }

        public int Count => ((ICollection<PagesWithFormat>)PagesWithFormat).Count;

        public bool IsReadOnly => ((ICollection<PagesWithFormat>)PagesWithFormat).IsReadOnly;

        public void Add(PagesWithFormat item)
        {
            ((ICollection<PagesWithFormat>)PagesWithFormat).Add(item);
        }

        public void Clear()
        {
            ((ICollection<PagesWithFormat>)PagesWithFormat).Clear();
        }

        public bool Contains(PagesWithFormat item)
        {
            return ((ICollection<PagesWithFormat>)PagesWithFormat).Contains(item);
        }

        public void CopyTo(PagesWithFormat[] array, int arrayIndex)
        {
            ((ICollection<PagesWithFormat>)PagesWithFormat).CopyTo(array, arrayIndex);
        }

        public IEnumerator<PagesWithFormat> GetEnumerator()
        {
            return ((IEnumerable<PagesWithFormat>)PagesWithFormat).GetEnumerator();
        }

        public int IndexOf(PagesWithFormat item)
        {
            return ((IList<PagesWithFormat>)PagesWithFormat).IndexOf(item);
        }

        public void Insert(int index, PagesWithFormat item)
        {
            ((IList<PagesWithFormat>)PagesWithFormat).Insert(index, item);
        }

        public bool Remove(PagesWithFormat item)
        {
            return ((ICollection<PagesWithFormat>)PagesWithFormat).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<PagesWithFormat>)PagesWithFormat).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)PagesWithFormat).GetEnumerator();
        }

        #endregion
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("PagesWithFormat", PagesWithFormat, typeof(List<PagesWithFormat>));
        }
    }
    [Serializable]
    [XmlRoot]
    [XmlInclude(typeof(FormatInfo))]
    [XmlInclude(typeof(PaperFormatName))]
    public class PagesWithFormat : IList<string>, INotifyPropertyChanged, ISerializable, ICollection
    {
        public PagesWithFormat()
        {
            FilePaths = new List<string>();
        }
        public string this[int index] { get => ((IList<string>)FilePaths)[index]; set => ((IList<string>)FilePaths)[index] = value; }
        private List<string> _filePaths;
        [XmlArray("Files")]
        [XmlArrayItem("File")]
        [XmlElement("Files")]
        public List<string> FilePathsInternal
        {
            get => _filePaths; set { _filePaths = value; }
        }
        public List<string> FilePaths
        {
            get => _filePaths.OrderBy(o => o).ToList(); set
            {
                _filePaths = value;
            }
        }
        [XmlElement("FormatInfo")]
        public FormatInfo FormatInfo { get; set; }
        public string PageCount { get => FilePaths.Count.ToString(); }
        public string Title { get => FormatInfo.Name.ToString(); }

        #region IList


        public int Count => ((ICollection<string>)FilePaths).Count;

        public bool IsReadOnly => ((ICollection<string>)FilePaths).IsReadOnly;

        public object SyncRoot => ((ICollection)FilePaths).SyncRoot;

        public bool IsSynchronized => ((ICollection)FilePaths).IsSynchronized;

        public void Add(string item)
        {
            ((ICollection<string>)FilePaths).Add(item);
        }

        public void Clear()
        {
            ((ICollection<string>)FilePaths).Clear();
        }

        public bool Contains(string item)
        {
            return ((ICollection<string>)FilePaths).Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            ((ICollection<string>)FilePaths).CopyTo(array, arrayIndex);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return ((IEnumerable<string>)FilePaths).GetEnumerator();
        }

        public int IndexOf(string item)
        {
            return ((IList<string>)FilePaths).IndexOf(item);
        }

        public void Insert(int index, string item)
        {
            ((IList<string>)FilePaths).Insert(index, item);
        }

        public bool Remove(string item)
        {
            return ((ICollection<string>)FilePaths).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<string>)FilePaths).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)FilePaths).GetEnumerator();
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public void AddRange(IEnumerable<string> toAdd)
        {
            _filePaths.AddRange(toAdd);
            NotifyPropertyChanged("FilePaths");
            NotifyPropertyChanged("PageCount");
            NotifyPropertyChanged("Count");
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", FormatInfo.Name, typeof(PaperFormatName));
            info.AddValue("Standard", FormatInfo.IsStandard, typeof(bool));
            info.AddValue("FilePaths", FilePathsInternal, typeof(List<string>));
            info.AddValue("FormatInfo", FormatInfo, typeof(FormatInfo));
        }

        public void CopyTo(Array array, int index)
        {
            ((ICollection)FilePaths).CopyTo(array, index);
        }
    }
}
