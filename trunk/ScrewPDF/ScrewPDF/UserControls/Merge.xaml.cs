using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.IO;

namespace ScrewPDF.UserControls
{
    public partial class Merge : UserControl
    {
        public Merge()
        {
            InitializeComponent();
        }

        private void uiBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.DefaultExt = ".pdf";
                dialog.Filter = "pdf documents (.pdf)|*.pdf";
                dialog.Multiselect = true;
                if (dialog.ShowDialog() == true)
                {
                    foreach (string file in dialog.FileNames)
                    {
                        uiInputFiles.Items.Add(file);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void uiInputFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && uiInputFiles.SelectedItem != null)
            {
                uiInputFiles.Items.Remove(uiInputFiles.SelectedItem);
            }
        }

        private void uiUp_Click(object sender, RoutedEventArgs e)
        {
            int index = uiInputFiles.SelectedIndex;
            if (index != -1 && index > 0)
            {
                object item = uiInputFiles.Items[index];
                uiInputFiles.Items.RemoveAt(index);
                uiInputFiles.Items.Insert(index - 1, item);
                uiInputFiles.SelectedIndex = index - 1;
                uiInputFiles.ScrollIntoView(item);
            }
        }

        private void uiDown_Click(object sender, RoutedEventArgs e)
        {
            int index = uiInputFiles.SelectedIndex;
            if (index != -1 && index < uiInputFiles.Items.Count - 1)
            {
                object item = uiInputFiles.Items[index];
                uiInputFiles.Items.RemoveAt(index);
                uiInputFiles.Items.Insert(index + 1, item);
                uiInputFiles.SelectedIndex = index + 1;
                uiInputFiles.ScrollIntoView(item);
            }
        }

        private void uiGo_Click(object sender, RoutedEventArgs e)
        {
            if (uiInputFiles.Items.Count <= 1)
            {
                MessageBox.Show("No PDFs to merge.");
                return;
            }
            try
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.DefaultExt = ".pdf";
                dialog.Filter = "PDF documents (.pdf)|*.pdf";
                if (dialog.ShowDialog() == true)
                {
                    PdfDocument outputDocument = new PdfDocument();
                    foreach (string file in uiInputFiles.Items)
                    {
                        if (File.Exists(file))
                        {
                            PdfDocument inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import);
                            for (int i = 0; i < inputDocument.Pages.Count; i++)
                            {
                                outputDocument.AddPage(inputDocument.Pages[i]);
                            }
                        }
                    }
                    outputDocument.Save(dialog.FileName);
                    MessageBox.Show("PDF file created.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}