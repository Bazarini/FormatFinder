using FormatFinderCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Format_Indexer
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Fields
        private BitmapImage _displayedImage;
        private string _selectedFile;
        private PagesWithFormat _selectedFormat;
        private Visibility _toolsVisible;
        private double _sbWidth;
        private double _sbHeight;
        #endregion

        #region Props
        public double SbHeight { get => _sbHeight; set => _sbHeight = value; }
        public double SbWidth { get => _sbWidth; set => _sbWidth = value; }
        public ObservableCollection<PagesWithFormat> Formats { get; set; }
        public PagesWithFormat SelectedFormat { get => _selectedFormat; set { _selectedFormat = value; NotifyPropertyChanged("SelectedFormat"); } }
        public BitmapImage DisplayedImage { get => _displayedImage; set { _displayedImage = value; NotifyPropertyChanged("DisplayedImage"); } }
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
                    SetOriginalSize();
                }
                GC.Collect();
            }
        }
        public Visibility ToolsVisible { get => _toolsVisible; set { _toolsVisible = value; NotifyPropertyChanged("ToolsVisible"); } }
        private ScaleTransform _imageTransform;
        public ScaleTransform ImageTransform { get => _imageTransform; set { _imageTransform = value; NotifyPropertyChanged("ImageTransform"); NotifyPropertyChanged("DisplayedImage"); } }
        #endregion

        #region Commands

        private RelayCommand _addFilesCommand;
        public RelayCommand AddFilesCommand
        {
            get
            {
                return _addFilesCommand ?? (new RelayCommand(obj =>
                {
                    OpenFileDialog dialog = new OpenFileDialog() { Multiselect = true };
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        string[] path = dialog.FileNames;
                        dialog.Dispose();
                        AddFiles(path);
                    }
                }));
            }
        }
        private RelayCommand _addFolderCommand;
        public RelayCommand AddFolderCommand
        {
            get
            {
                return _addFolderCommand ?? (new RelayCommand(obj =>
                {
                    FolderBrowserDialog dialog = new FolderBrowserDialog();
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        string path = dialog.SelectedPath;
                        dialog.Dispose();
                        AddFiles(path);
                    }
                }));
            }
        }
        private RelayCommand _replaceFilesCommand;
        public RelayCommand ReplaceFilesCommand
        {
            get
            {
                return _replaceFilesCommand ?? (new RelayCommand(obj =>
                {
                    if (Formats != null && Formats.Count == 0)
                        if (System.Windows.MessageBox.Show("All files will be replaced. Continue?", "Replace images?", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) != MessageBoxResult.Yes)
                            return;
                    OpenFileDialog dialog = new OpenFileDialog() { Multiselect = true, Filter = "All Files|*.*|Images|*.jpg;*.jpeg;*.png;*.tif;*.tiff;*.bmp" };

                    if (dialog.ShowDialog() == DialogResult.OK)
                        LoadFile(dialog.FileNames);
                }));
            }
        }
        private RelayCommand _replaceFolderCommand;
        public RelayCommand ReplaceFolderCommand
        {
            get
            {
                return _replaceFolderCommand ?? (new RelayCommand(obj =>
                {
                    if (Formats != null && Formats.Count == 0)
                        if (System.Windows.MessageBox.Show("All files will be replaced. Continue?", "Replace images?", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) != MessageBoxResult.Yes)
                            return;
                    FolderBrowserDialog dialog = new FolderBrowserDialog();
                    if (dialog.ShowDialog() == DialogResult.OK)
                        LoadFile(dialog.SelectedPath);
                }));
            }
        }
        private RelayCommand _changeToolsVisibility;
        public RelayCommand ChangeToolsVisibility
        {
            get
            {
                return _changeToolsVisibility ?? (new RelayCommand(obj =>
                {
                    if (ToolsVisible == Visibility.Collapsed)
                        ToolsVisible = Visibility.Visible;
                    else ToolsVisible = Visibility.Collapsed;
                }));
            }
        }
        private RelayCommand _saveAsCommand;
        public RelayCommand SaveAsCommand
        {
            get
            {
                return _saveAsCommand ?? (new RelayCommand(obj => SaveAs()));
            }
        }
        private RelayCommand _exitCommand;
        public RelayCommand ExitCommand
        {
            get
            {
                return _exitCommand ?? (new RelayCommand(obj => Environment.Exit(0)));
            }
        }
        private RelayCommand _fitToWindowCommand;
        public RelayCommand FitToWindowCommand
        {
            get
            {
                return _fitToWindowCommand ?? (new RelayCommand(obj => FitToWindow()));
            }
        }
        private RelayCommand _fitToWidthCommand;
        public RelayCommand FitToWidthCommand
        {
            get
            {
                return _fitToWidthCommand ?? (new RelayCommand(obj => FitToWidth()));
            }
        }
        private RelayCommand _fitToHeightCommand;
        public RelayCommand FitToHeightCommand
        {
            get
            {
                return _fitToHeightCommand ?? (new RelayCommand(obj => FitToHeight()));
            }
        }
        private RelayCommand _zoomInCommand;
        public RelayCommand ZoomInCommand
        {
            get
            {
                return _zoomInCommand ?? (new RelayCommand(obj => Zoom(1.2)));
            }
        }
        private RelayCommand _zoomOutCommand;
        public RelayCommand ZoomOutCommand
        {
            get
            {
                return _zoomOutCommand ?? (new RelayCommand(obj => Zoom(1 / 1.2)));
            }
        }
        private RelayCommand _setOriginalSizeCommand;
        public RelayCommand SetOriginalSizeCommand
        {
            get
            {
                return _setOriginalSizeCommand ?? (new RelayCommand(obj => SetOriginalSize()));
            }
        }
        #endregion

        #region ctor
        public MainWindowViewModel()
        {
            _displayedImage = new BitmapImage();
            ToolsVisible = Visibility.Collapsed;
        }
        #endregion

        #region Methods

        private void Zoom(double multiplier)
        {
            if (!InitScale())
                return;
            ImageTransform.ScaleX *= multiplier;
            ImageTransform.ScaleY *= multiplier;
            NotifyPropertyChanged("DisplayedImage");
        }
        private void SetOriginalSize()
        {
            if (!InitScale())
                return;
            ImageTransform.ScaleX = 1;
            ImageTransform.ScaleY = 1;
            NotifyPropertyChanged("DisplayedImage");
        }
        private void FitToWindow()
        {
            if (!InitScale())
                return;
            var multiplier = Math.Min(SbWidth / (DisplayedImage.Width * ImageTransform.ScaleX), SbHeight / (DisplayedImage.Height * ImageTransform.ScaleY));
            Zoom(multiplier);
        }

        private void FitToHeight()
        {
            if (!InitScale())
                return;
            var multiplier = SbHeight / (DisplayedImage.Height * ImageTransform.ScaleY);
            Zoom(multiplier);
        }
        private void FitToWidth()
        {
            if (!InitScale())
                return;
            var multiplier = SbWidth / (DisplayedImage.Width * ImageTransform.ScaleX);
            Zoom(multiplier);
        }
        private bool InitScale()
        {
            if (DisplayedImage.UriSource == null)
                return false;
            if (ImageTransform == null)
                ImageTransform = new ScaleTransform();
            return true;
        }

        public void AddFiles(string[] filesInput)
        {
            if (Formats == null)
                Formats = new ObservableCollection<PagesWithFormat>();
            List<PagesWithFormat> tempFormats = new List<PagesWithFormat>();
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
            foreach (IGrouping<FormatInfo, KeyValuePair<string, FormatInfo>> item in temp.GroupBy(g => g.Value))
                tempFormats.Add(new PagesWithFormat() { FormatInfo = item.Key, FilePaths = item.Select(s => s.Key).ToList() });


            foreach (PagesWithFormat paperFormat in tempFormats)
            {
                PagesWithFormat existingFormat = Formats.FirstOrDefault(f => f.FormatInfo.Name == paperFormat.FormatInfo.Name);
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
                Formats = new ObservableCollection<PagesWithFormat>();
            else
                Formats.Clear();
            AddFiles(files);
        }
        public void LoadFile(string path)
        {
            if (Formats == null)
                Formats = new ObservableCollection<PagesWithFormat>();
            else
                Formats.Clear();
            AddFiles(path);
        }
        public void SaveAs()
        {
            SaveOptionSelector optionSelectorWindow = new SaveOptionSelector();
            if (optionSelectorWindow.ShowDialog().Value)
            {
                if (!string.IsNullOrEmpty(optionSelectorWindow.vm.SelectedPath))
                    foreach (var option in optionSelectorWindow.vm.saveOptions)
                    {

                        var serObj = new PagesWithFormatCollection() { PagesWithFormat = Formats.ToList() };                        
                        SaveHelper.SaveAs(optionSelectorWindow.vm.SelectedPath, option, serObj, typeof(PagesWithFormatCollection));
                    }
                else
                    System.Windows.MessageBox.Show($"Path is not valid: \"{optionSelectorWindow.vm.SelectedPath}\"");
            }
        }
        #endregion

        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
        #endregion

    }
}
