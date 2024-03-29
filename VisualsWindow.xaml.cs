﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OverlayControl
{
    /// <summary>
    /// Window containing the visuals to be used for a stream overlay, including characters, moon styles and country flags.
    /// </summary>
    public partial class VisualsWindow : Window, IComponentConnector
    {
        /// <summary>
        /// Constructor for the VisualsWindow class, containing the visuals to be used for a stream overlay, including characters, moon styles and country flags.
        /// </summary>
        /// <param name="cutIn01">Image representing player 1's character.</param>
        /// <param name="moon01">Image representing player 1's moon style.</param>
        /// <param name="cutIn02">Image representing player 2's character.</param>
        /// <param name="moon02">Image representing player 1's moon style.</param>
        /// <param name="flag01">Image representing player 1's country flag.</param>
        /// <param name="flag02">Image representing player 2's country flag.</param>
        public VisualsWindow(BitmapImage cutIn01, BitmapImage moon01, BitmapImage cutIn02, BitmapImage moon02, BitmapImage flag01, BitmapImage flag02)
        {
            InitializeComponent();
            Height = 768.0;
            Width = 1024.0;
            imgCutIn1.Source = (ImageSource)cutIn01;
            imgMoon1.Source = (ImageSource)moon01;
            imgFlag1.Source = (ImageSource)flag01;
            imgCutIn2.Source = (ImageSource)cutIn02;
            imgMoon2.Source = (ImageSource)moon02;
            imgFlag2.Source = (ImageSource)flag02;
        }

        /// <summary>
        /// Updates the images shown in the VisualsWindow.
        /// </summary>
        /// <param name="cutIn01">Image representing player 1's character.</param>
        /// <param name="moon01">Image representing player 1's moon style.</param>
        /// <param name="cutIn02">Image representing player 2's character.</param>
        /// <param name="moon02">Image representing player 1's moon style.</param>
        /// <param name="flag01">Image representing player 1's country flag.</param>
        /// <param name="flag02">Image representing player 2's country flag.</param>
        /// <returns>Whether the operation was successful or not.</returns>
        public bool ChangeSource(BitmapImage cutIn01, BitmapImage moon01, BitmapImage cutIn02, BitmapImage moon02, BitmapImage flag01, BitmapImage flag02)
        {
            try
            {
                imgCutIn1.Source = (ImageSource)cutIn01;
                imgMoon1.Source = (ImageSource)moon01;
                imgFlag1.Source = (ImageSource)flag01;
                imgFlag1.Stretch = Stretch.Uniform;
                imgFlag1.HorizontalAlignment = HorizontalAlignment.Center;
                imgCutIn2.Source = (ImageSource)cutIn02;
                imgMoon2.Source = (ImageSource)moon02;
                imgFlag2.Source = (ImageSource)flag02;
                imgFlag2.Stretch = Stretch.Uniform;
                imgFlag2.HorizontalAlignment = HorizontalAlignment.Center;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Cancelable event fired when attempting to close the window containing the stream overlay visuals.
        /// </summary>
        /// <param name="sender">The control that was used to fire this event.</param>
        /// <param name="e">Provides data for the cancelable event.</param>
        private void wndVisuals_Closing(object sender, CancelEventArgs e)
        {
            // Cancel manual closing of this window
            e.Cancel = true;
        }
    }
}
