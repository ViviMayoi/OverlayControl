// Decompiled with JetBrains decompiler
// Type: OverlayControl.Match
// Assembly: OverlayControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7DCA1402-0FB1-4086-89FB-0ABEDB51AD19
// Assembly location: D:\Vivi\Drive\Stream\OverlayControl.exe

using System.Collections.Generic;

namespace OverlayControl
{
  public class Match
  {
    public string Player1 { get; set; }

    public string Player2 { get; set; }

    public List<string> Characters1 { get; set; }

    public List<string> Characters2 { get; set; }

    public string Round { get; set; }

    public string Tournament { get; set; }

    public Match(string p1, string p2, string c1, string c2, string r, string t)
    {
      this.Player1 = p1;
      this.Player2 = p2;
      this.Characters1 = new List<string>() { c1 };
      this.Characters2 = new List<string>() { c2 };
      this.Round = r;
      this.Tournament = t;
    }

    public bool IsNewMatch(string p1, string p2)
    {
      if (p1 == this.Player1 && p2 == this.Player2)
        return false;
      return !(p2 == this.Player1) || !(p1 == this.Player2);
    }

    public override string ToString()
    {
      string str1 = this.Round + " - " + this.Player1;
      if (this.Characters1.Count == 1)
        str1 = str1 + " (" + this.Characters1[0] + ")";
      else if (this.Characters1.Count != 0)
      {
        string str2 = str1 + " (";
        for (int index = 0; index < this.Characters1.Count; ++index)
        {
          str2 += this.Characters1[index];
          if (index != this.Characters1.Count - 1)
            str2 += ", ";
        }
        str1 = str2 + ")";
      }
      string str3 = str1 + " vs " + this.Player2;
      if (this.Characters2.Count == 1)
        str3 = str3 + " (" + this.Characters2[0] + ")";
      else if (this.Characters2.Count != 0)
      {
        string str2 = str3 + " (";
        for (int index = 0; index < this.Characters2.Count; ++index)
        {
          str2 += this.Characters2[index];
          if (index != this.Characters2.Count - 1)
            str2 += ", ";
        }
        str3 = str2 + ")";
      }
      return str3 + "\n";
    }
  }
}
