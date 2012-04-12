using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Advanced;

namespace ScrewPDF.UserControls
{
    /// <summary>
    /// Interaction logic for ExtractImage.xaml
    /// </summary>
    public partial class ExtractImage : UserControl
    {
        string inputFile = string.Empty;

        public ExtractImage()
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
                int imageCount = 0;
                var dialog = new System.Windows.Forms.FolderBrowserDialog();
                dialog.SelectedPath = System.IO.Path.GetDirectoryName(inputFile);
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    PdfDocument document = PdfReader.Open(inputFile);
                    foreach (PdfPage page in document.Pages)
                    {
                        PdfDictionary resources = page.Elements.GetDictionary("/Resources");
                        if (resources != null)
                        {
                            PdfDictionary xObjects = resources.Elements.GetDictionary("/XObject");
                            if (xObjects != null)
                            {
                                ICollection<PdfItem> items = xObjects.Elements.Values;
                                foreach (PdfItem item in items)
                                {
                                    PdfReference reference = item as PdfReference;
                                    if (reference != null)
                                    {
                                        PdfDictionary xObject = reference.Value as PdfDictionary;
                                        if (xObject != null && xObject.Elements.GetString("/Subtype") == "/Image")
                                        {
                                            ExportImage(xObject, dialog.SelectedPath, ref imageCount);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    MessageBox.Show(string.Format("{0} images extracted.", imageCount));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("The file is not a valid PDF.");
            }
        }

        private void ExportImage(PdfDictionary image, string selectedPath, ref int imageCount)
        {
            string filter = image.Elements.GetName("/Filter");
            switch (filter)
            {
                case "/DCTDecode": ExportJpegImage(image, selectedPath, ref imageCount); break;
                case "/FlateDecode": ExportPngImage(image, selectedPath, ref imageCount); break;
            }
        }

        private void ExportJpegImage(PdfDictionary image, string selectedPath, ref int imageCount)
        {
            byte[] stream = image.Stream.Value;
            using (FileStream fs = new FileStream(string.Format("{0}\\Image_{1}.jpeg", selectedPath, imageCount++), FileMode.Create, FileAccess.Write))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(stream);
                }
            }

        }

        private void ExportPngImage(PdfDictionary image, string selectedPath, ref int imageCount)
        {
            //TODO
        }
    }
}
