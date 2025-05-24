using Microsoft.Win32;
using System.Drawing;
using System.IO;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Financiera_GUI.Utilities
{
    public partial class ucDocumentButton : UserControl
    {
        private string _text;
        public string Text
        {
            get { return _text; }
            set 
            { 
                _text = value;
                textLb.Content = value;
            }
        }

        private string _acceptedFile;
        public string AcceptedFile
        {
            set
            {
                if (value == "image" || value == "pdf")
                {
                    _acceptedFile = value;
                }
            }
        }

        public int DocumentationId { get; set; }
        public string DocumentationName { get; set; }

        public byte[] File;
        public string FileName;

        public bool selectable = true;

        public void SetImage(string path)
        {
            image.Source = new BitmapImage(new Uri(path));
        }

        public ucDocumentButton()
        {
            InitializeComponent();
        }

        private void SelectFile(object sender, MouseButtonEventArgs e)
        {
            if (!selectable)
            {
                Download();
            }
            
            string filter = "";

            switch (_acceptedFile)
            {
                case "image":
                    filter = "Image files (*.png, *.jpg, *.jpeg)|*.png;*.jpg;*.jpeg";
                    break;
                case "pdf":
                    filter = "Pdf files (*.pdf)|*.pdf";
                    break;
            }

            var fileDialog = new OpenFileDialog()
            {
                Filter = filter,
                Title = "Open text file"
            };

            if (fileDialog.ShowDialog() == true)
            {
                try
                {
                    File = System.IO.File.ReadAllBytes(fileDialog.FileName);
                    FileName = fileDialog.FileName;
                    button.Background = Brushes.LightGreen;
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private void Download()
        {
            string newFileName = $"{FileName.Substring(0, 2)}{DocumentationName}{Path.GetExtension(FileName)}";
            System.IO.File.WriteAllBytes("./creditDocuments/"+newFileName, File);
        }
    }
}
