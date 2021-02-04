using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OverlayControl
{
    public partial class CharacterCutIn : Window, IComponentConnector
    {
        public CharacterCutIn(BitmapImage cutInSource, BitmapImage moonSource)
        {
            this.InitializeComponent();
            this.Height = 512.0;
            this.Width = 512.0;
            this.imgCutIn.Source = (ImageSource)cutInSource;
            this.imgMoon.Source = (ImageSource)moonSource;
        }

        public bool ChangeSource(BitmapImage cutInSource, BitmapImage moonSource)
        {
            try
            {
                this.imgCutIn.Source = (ImageSource)cutInSource;
                this.imgMoon.Source = (ImageSource)moonSource;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void wndCutIn_Closing(object sender, CancelEventArgs e)
        {
            if (MainWindow.IsClosing)
                return;
            e.Cancel = true;
        }
    }
}
