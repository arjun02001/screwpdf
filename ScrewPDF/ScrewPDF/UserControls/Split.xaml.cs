using System;
using System.Windows;
using System.Windows.Controls;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Microsoft.Win32;
using System.IO;

namespace ScrewPDF.UserControls
{
    public partial class Split : UserControl
    {
        string inputFile = string.Empty;

        public Split()
        {
            InitializeComponent();
        }

        private void uiBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.DefaultExt = ".pdf";
                dialog.Filter = "PDF documents (.pdf)|*.pdf";
                if (dialog.ShowDialog() == true)
                {
                    uiInputFile.Text = dialog.FileName.Trim();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void uiGo_Click(object sender, RoutedEventArgs e)
        {
            inputFile = uiInputFile.Text.Trim();
            if (!File.Exists(inputFile))
            {
                MessageBox.Show("Please select a file.");
                return;
            }
            try
            {
                var dialog = new System.Windows.Forms.FolderBrowserDialog();
                dialog.SelectedPath = System.IO.Path.GetDirectoryName(inputFile);
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    PdfDocument inputDocument = PdfReader.Open(inputFile, PdfDocumentOpenMode.Import);
                    string outputFilename = System.IO.Path.GetFileNameWithoutExtension(inputFile);
                    for (int i = 0; i < inputDocument.PageCount; i++)
                    {
                        PdfDocument outputDocument = new PdfDocument();
                        outputDocument.Version = inputDocument.Version;
                        outputDocument.Info.Title =inputDocument.Info.Title;
                        outputDocument.Info.Creator = inputDocument.Info.Creator;
                        outputDocument.AddPage(inputDocument.Pages[i]);
                        outputDocument.Save(String.Format("{0}\\{1}_{2}.pdf", dialog.SelectedPath, outputFilename, (i + 1)));
                    }
                    MessageBox.Show("PDF files created.");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("The file is not a valid PDF.");
            }
        }
    }
}