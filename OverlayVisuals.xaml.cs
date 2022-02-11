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
    public partial class OverlayVisuals : Window, IComponentConnector
    {
        public OverlayVisuals(BitmapImage cutIn01, BitmapImage moon01, BitmapImage cutIn02, BitmapImage moon02, BitmapImage flag01, BitmapImage flag02)
        {
            this.InitializeComponent();
            this.Height = 768.0;
            this.Width = 1024.0;
            this.imgCutIn1.Source = (ImageSource)cutIn01;
            this.imgMoon1.Source = (ImageSource)moon01;
            this.imgFlag1.Source = (ImageSource)flag01;
            this.imgCutIn2.Source = (ImageSource)cutIn02;
            this.imgMoon2.Source = (ImageSource)moon02;
            this.imgFlag2.Source = (ImageSource)flag02;
        }

        public bool ChangeSource(BitmapImage cutIn01, BitmapImage moon01, BitmapImage cutIn02, BitmapImage moon02, BitmapImage flag01, BitmapImage flag02)
        {
            try
            {
                this.imgCutIn1.Source = (ImageSource)cutIn01;
                this.imgMoon1.Source = (ImageSource)moon01;
                this.imgFlag1.Source = (ImageSource)flag01;
                this.imgFlag1.Stretch = Stretch.Uniform;
                this.imgFlag1.HorizontalAlignment = HorizontalAlignment.Center;
                this.imgCutIn2.Source = (ImageSource)cutIn02;
                this.imgMoon2.Source = (ImageSource)moon02;
                this.imgFlag2.Source = (ImageSource)flag02;
                this.imgFlag2.Stretch = Stretch.Uniform;
                this.imgFlag2.HorizontalAlignment = HorizontalAlignment.Center;
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
