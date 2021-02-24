using FormatFinderCore;
using System.Collections.Generic;
using System.ComponentModel;

namespace Format_Indexer
{
    public class SaveOptionSelectorVM : INotifyPropertyChanged
    {
        private bool _xml;

        public bool XML
        {
            get { return _xml; }
            set { _xml = value; NotifyPropertyChanged("XML"); }
        }
        private bool _csv;

        public bool CSV
        {
            get { return _csv; }
            set { _csv = value; NotifyPropertyChanged("CSV"); }
        }
        private bool _json;

        public bool JSON
        {
            get { return _json; }
            set { _json = value; NotifyPropertyChanged("JSON"); }
        }
        private bool _txt;

        public bool TXT
        {
            get { return _txt; }
            set { _txt = value; NotifyPropertyChanged("TXT"); }
        }
        private string _path;

        public string SelectedPath
        {
            get { return _path; }
            set
            {
                _path = value;
                NotifyPropertyChanged("SelectedPath");
            }
        }
        public SaveOption[] saveOptions
        {
            get
            {
                List<SaveOption> so = new List<SaveOption>();
                if (TXT)
                    so.Add(SaveOption.TXT);
                if (XML)
                    so.Add(SaveOption.XML);
                if (CSV)
                    so.Add(SaveOption.CSV);
                if (JSON)
                    so.Add(SaveOption.JSON);
                return so.ToArray();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
