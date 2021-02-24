using FormatFinderCore;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FormatFinderCore
{
    public class FormatCollection : IList<string>, INotifyPropertyChanged
    {
        public string this[int index] { get => ((IList<string>)FilePaths)[index]; set => ((IList<string>)FilePaths)[index] = value; }
        private List<string> _filePaths;
        public List<string> FilePathsInternal
        {
            get => _filePaths; 
            set
            {
                _filePaths = value;
                NotifyPropertyChanged("FilePaths");
            }
        }
        public List<string> FilePaths { get => _filePaths.OrderBy(o => o).ToList(); set 
            { 
                _filePaths = value; 
            } 
        }
        public FormatInfo FormatInfo { get; set; }
        public string PageCount { get => FilePaths.Count.ToString(); }
        public string Title { get => FormatInfo.Name.ToString(); }

        public int Count => ((ICollection<string>)FilePaths).Count;

        public bool IsReadOnly => ((ICollection<string>)FilePaths).IsReadOnly;

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
    }
}
