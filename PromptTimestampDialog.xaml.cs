using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace OverlayControl
{
    /// <summary>
    /// Window acting as a prompt for the user to enter a timestamp.
    /// </summary>
    public partial class PromptTimestampDialog : Window
    {
        public TimeSpan GivenTime { get => new TimeSpan(int.Parse(txtHours.Text), int.Parse(txtMinutes.Text), int.Parse(txtSeconds.Text)); }

        /// <summary>
        /// Constructor for the PromptTimestampDialog window, acting as a prompt for the user to enter a timestamp.
        /// </summary>
        public PromptTimestampDialog()
        {
            InitializeComponent();
        }

        #region Text Boxes
        /// <summary>
        /// Event fired when a text box that should only contain numbers is typed into.
        /// </summary>
        /// <param name="sender">The text box that fired this event.</param>
        /// <param name="e">Contains state information and event data.</param>
        private void txtNumber_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) =>
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);

        /// <summary>
        /// Event fired when a text box that should only contain numbers is pasted into.
        /// </summary>
        /// <param name="sender">The text box that fired this event.</param>
        /// <param name="e">Contains state information and event data.</param>
        private void txtNumber_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            // Check if data is a string
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                // Check if the data string only contains numbers; if not, cancel
                if (new Regex("[^0-9]+").IsMatch((string)e.DataObject.GetData(typeof(string))))
                    e.CancelCommand();
            }
            // If data is not a string, cancel paste
            else
                e.CancelCommand();
        }

        /// <summary>
        /// Event fired when a text box that should only contain numbers has data dragged down into it via mouse.
        /// </summary>
        /// <param name="sender">The text box that fired this event.</param>
        /// <param name="e">Contains state information and event data.</param>
        private void txtNumber_PreviewDragEnter(object sender, DragEventArgs e)
        {
            // Check if data is a string
            if (e.Data.GetDataPresent(typeof(string)))
            {
                // Check if the data string only contains numbers; if not, cancel
                e.Handled = new Regex("[^0-9]+").IsMatch((string)e.Data.GetData(typeof(string)));
            }
            // If data is not a string, cancel drag-down
            else
                e.Handled = true;
        }
        #endregion

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
            txtHours.Focus();
        }
    }
}
