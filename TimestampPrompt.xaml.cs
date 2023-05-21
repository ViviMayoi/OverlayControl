using System;
using System.Windows;

namespace OverlayControl
{
    public partial class TimestampPrompt : Window
    {
        public string GivenTime { get => txtVodTime.Text; }

        public TimestampPrompt()
        {
            InitializeComponent();
        }

        private void btnFinalize_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void prompt_ContentRendered(object sender, EventArgs e)
        {
            txtVodTime.Focus();
        }
    }
}
