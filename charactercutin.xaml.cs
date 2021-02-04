﻿// Decompiled with JetBrains decompiler
// Type: OverlayControl.CharacterCutIn
// Assembly: OverlayControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7DCA1402-0FB1-4086-89FB-0ABEDB51AD19
// Assembly location: D:\Vivi\Drive\Stream\OverlayControl.exe

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
    private const int HEIGHT = 512;
    private const int WIDTH = 512;

    public CharacterCutIn(BitmapImage cutInSource, BitmapImage moonSource)
    {
      this.InitializeComponent();
            this.Height = 512.0;
      this.Width = 512.0;
      this.imgCutIn.Source = (ImageSource) cutInSource;
      this.imgMoon.Source = (ImageSource) moonSource;
    }

    public bool ChangeSource(BitmapImage cutInSource, BitmapImage moonSource)
    {
      try
      {
        this.imgCutIn.Source = (ImageSource) cutInSource;
        this.imgMoon.Source = (ImageSource) moonSource;
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
