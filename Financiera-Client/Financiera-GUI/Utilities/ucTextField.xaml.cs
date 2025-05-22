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

namespace Financiera_GUI.Utilities
{
    public partial class ucTextField : UserControl
    {
        private string _text;
        private uint _max_len;
        public string Text 
        { 
            get => textField.Text; 
            set 
            {
                content.Content = value;
                _text = value;
            } 
        }
        public uint MaxLen
        {
            get => _max_len;
            set
            {
                textField.MaxLength = (int)value;
                _max_len = value;
            }
        }

        public ucTextField()
        {
            InitializeComponent();
        }

        public void showError(string message)
        {
            errorLabel.Content = message;
            errorLabel.Visibility = Visibility.Visible;
        }
    }
}
