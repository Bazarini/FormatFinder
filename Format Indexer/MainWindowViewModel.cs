using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using FormatFinderCore;

namespace Format_Indexer
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<FormatCollection> _formats;
        public ObservableCollection<FormatCollection> Formats { get => _formats; set { _formats = value; } }
        FormatCollection _selectedFormat;
        public FormatCollection SelectedFormat { get => _selectedFormat; set { _selectedFormat = value; NotifyPropertyChanged("SelectedFormat"); } }

        BitmapImage _displayedImage;
        public BitmapImage DisplayedImage { get => _displayedImage; set { _displayedImage = value; NotifyPropertyChanged("DisplayedImage"); } }
        private string _selectedFile;

        public string SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                NotifyPropertyChanged("SelectedFile");
                if (!string.IsNullOrEmpty(value))
                {
                    DisplayedImage = new BitmapImage();
                    DisplayedImage.BeginInit();
                    DisplayedImage.CacheOption = BitmapCacheOption.OnLoad;
                    DisplayedImage.UriSource = new Uri(value);
                    DisplayedImage.EndInit();
                }
                GC.Collect();
            }
        }

        #region ctor
        public MainWindowViewModel()
        {
            _displayedImage = new BitmapImage();
        }
        #endregion
        public void AddFiles(string[] filesInput)
        {
            var d = Dispatcher.CurrentDispatcher;

            if (Formats == null)
                Formats = new ObservableCollection<FormatCollection>();
            List<FormatCollection> tempFormats = new List<FormatCollection>();
            List<string> files = new List<string>();
            string[] extensions = new[] { ".tif", ".tiff", ".jpeg", ".png", ".jpg" };

            Dictionary<string, FormatInfo> temp = new Dictionary<string, FormatInfo>();
            foreach (string item in filesInput)
            {
                if (extensions.Contains(Path.GetExtension(item)))
                {
                    temp.Add(item, FormatExtractor.GetFormatName(file: item));
                }
            }
            foreach (var item in temp.GroupBy(g => g.Value))
                tempFormats.Add(new FormatCollection() { FormatInfo = item.Key, FilePaths = item.Select(s => s.Key).ToList() });


            foreach (var paperFormat in tempFormats)
            {
                var existingFormat = Formats.FirstOrDefault(f => f.FormatInfo.Name == paperFormat.FormatInfo.Name);
                if (existingFormat != null)
                    existingFormat.AddRange(paperFormat.FilePaths);
                else
                    Formats.Add(paperFormat);
            }
            NotifyPropertyChanged("Formats");
            NotifyPropertyChanged("SelectedFormat");

        }
        public void AddFiles(string path)
        {
            AddFiles(Directory.GetFiles(path, "*.*", SearchOption.AllDirectories));
        }
        public void LoadFile(string[] files)
        {
            if (Formats == null)
                Formats = new ObservableCollection<FormatCollection>();
            else
                Formats.Clear();
            AddFiles(files);
        }
        public void LoadFile(string path)
        {
            if (Formats == null)
                Formats = new ObservableCollection<FormatCollection>();
            else
                Formats.Clear();
            AddFiles(path);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        public void SaveAs(string path, SaveOption[] saveOptions)
        {
            foreach (var option in saveOptions)
            {
                SaveHelper.SaveAs(path, option, Formats.ToList(), typeof(List<FormatCollection>));
            }
        }
        
    }
}
