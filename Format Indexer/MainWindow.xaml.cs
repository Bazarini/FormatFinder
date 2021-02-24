using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace Format_Indexer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = new MainWindowViewModel();
            DataContext = vm;

        }

        private void ReplaceFolder(object sender, RoutedEventArgs e)
        {
            if (vm.Formats != null && vm.Formats.Count == 0)
                if (System.Windows.MessageBox.Show("All files will be replaced. Continue?", "Replace images?", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) != MessageBoxResult.Yes)
                    return;
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                vm.LoadFile(dialog.SelectedPath);
        }
        private void AddFolder(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var path = dialog.SelectedPath;
                dialog.Dispose();
                vm.AddFiles(path);
            }
        }
        private void ReplaceFiles(object sender, RoutedEventArgs e)
        {
            if (vm.Formats != null && vm.Formats.Count == 0)
                if (System.Windows.MessageBox.Show("All files will be replaced. Continue?", "Replace images?", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) != MessageBoxResult.Yes)
                    return;
            OpenFileDialog dialog = new OpenFileDialog() { Multiselect = true, Filter = "All Files|*.*|Images|*.jpg;*.jpeg;*.png;*.tif;*.tiff;*.bmp" };
            
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                vm.LoadFile(dialog.FileNames);
        }
        private void AddFiles(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog() { Multiselect = true };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var path = dialog.FileNames;
                dialog.Dispose();
                vm.AddFiles(path);
            }
        }
        private void Exit(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void SaveAs(object sender, RoutedEventArgs e)
        {
            SaveOptionSelector optionSelectorWindow = new SaveOptionSelector();
            if (optionSelectorWindow.ShowDialog().Value)
            {
                vm.SaveAs(optionSelectorWindow.vm.SelectedPath, optionSelectorWindow.vm.saveOptions);
            }
        }
    }
}
