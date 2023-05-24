using System;
using System.Windows;

namespace OverlayControl
{
    /// <summary>
    /// Window acting as a prompt for the user to enter a timestamp.
    /// </summary>
    public partial class PromptTimestampDialog : Window
    {
        public string GivenTime { get => txtVodTime.Text; }

        /// <summary>
        /// Constructor for the PromptTimestampDialog window, acting as a prompt for the user to enter a timestamp.
        /// </summary>
        public PromptTimestampDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event fired when button btnFinalize is clicked.
        /// </summary>
        /// <param name="sender">The button that was pressed to fire this event.</param>
        /// <param name="e">Contains state information and event data.</param>
        private void btnFinalize_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        /// <summary>
        /// Event fired when the PromptTimestampDialog window is rendered.
        /// </summary>
        /// <param name="sender">The control that fired this event.</param>
        /// <param name="e">Unused event arguments object.</param>
        private void wndTimestampPrompt_ContentRendered(object sender, EventArgs e)
        {
            // Allow the user to type into the textbox immediately by putting it in focus when the window is rendered
            txtVodTime.Focus();
        }
    }
}
