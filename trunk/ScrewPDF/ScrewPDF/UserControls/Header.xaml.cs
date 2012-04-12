using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScrewPDF.UserControls
{
    /// <summary>
    /// Interaction logic for Header.xaml
    /// </summary>
    public partial class Header : UserControl
    {
        public Header()
        {
            InitializeComponent();
        }

        private void uiAbout_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Developed by Arjun Mukherji. Send feedbacks to arjun02001@gmail.com.");
        }
    }
}
