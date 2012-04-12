using System.Windows;
using ScrewPDF.UserControls;

namespace ScrewPDF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void uiMerge_Click(object sender, RoutedEventArgs e)
        {
            uiOperation.Children.Clear();
            uiOperation.Children.Add(new Merge());
        }

        private void uiSplit_Click(object sender, RoutedEventArgs e)
        {
            uiOperation.Children.Clear();
            uiOperation.Children.Add(new Split());
        }

        private void uiExtractImages_Click(object sender, RoutedEventArgs e)
        {
            uiOperation.Children.Clear();
            uiOperation.Children.Add(new ExtractImage());
        }
    }
}
