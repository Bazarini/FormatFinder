using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Format_Indexer
{
    /// <summary>
    /// Interaction logic for SaveOptionSelector.xaml
    /// </summary>
    public partial class SaveOptionSelector : Window
    {
        public SaveOptionSelectorVM vm;
        public SaveOptionSelector()
        {
            InitializeComponent();
            vm = new SaveOptionSelectorVM();
            DataContext = vm;
        }

        private void SelectPath(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { DefaultExt = "" };
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                vm.SelectedPath = saveFileDialog.FileName;
            }
            saveFileDialog.Dispose();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
        private void OK(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

    }
}
