using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace OverlayControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IComponentConnector
    {
        public static bool IsClosing = false;
        public static List<string> CharacterList = new List<string>()
    {
      "Akiha Tohno",
      "Akiha Vermillion",
      "Aoko Aozaki",
      "Archetype-Earth",
      "Arcueid Brunestud",
      "Ciel",
      "Hisui",
      "Hisui & Kohaku",
      "Kohaku",
      "Koha & Mech",
      "Kouma Kishima",
      "Len",
      "Mech-Hisui",
      "Michael Roa Valdamjong",
      "Miyako Arima",
      "Neco & Mech",
      "Neco-Arc",
      "Neco-Arc Chaos",
      "Nrvnqsr Chaos",
      "Powered Ciel",
      "Red Arcueid",
      "Riesbyfe Stridberg",
      "Satsuki Yumizuka",
      "Seifuku Akiha",
      "Shiki Nanaya",
      "Shiki Ryougi",
      "Shiki Tohno",
      "Sion Eltnam Atlasia",
      "Sion Tatari",
      "Wallachia",
      "White Len"
    };

        public static string[] CharacterArray = new string[0x64];

        public Dictionary<string, string> Shorthands = new Dictionary<string, string>()
    {
      {
        "Akiha Tohno",
        "Akiha"
      },
      {
        "Akiha Vermillion",
        "VAkiha"
      },
      {
        "Aoko Aozaki",
        "Aoko"
      },
      {
        "Archetype-Earth",
        "Hime"
      },
      {
        "Arcueid Brunestud",
        "Arcueid"
      },
      {
        "Ciel",
        "Ciel"
      },
      {
        "Hisui",
        "Hisui"
      },
      {
        "Hisui & Kohaku",
        "Maids"
      },
      {
        "Kohaku",
        "Kohaku"
      },
      {
        "Koha & Mech",
        "Kohamech"
      },
      {
        "Kouma Kishima",
        "Kouma"
      },
      {
        "Len",
        "Len"
      },
      {
        "Mech-Hisui",
        "Mech"
      },
      {
        "Michael Roa Valdamjong",
        "Roa"
      },
      {
        "Miyako Arima",
        "Miyako"
      },
      {
        "Neco & Mech",
        "Necomech"
      },
      {
        "Neco-Arc",
        "Neco-Arc"
      },
      {
        "Neco-Arc Chaos",
        "NAC"
      },
      {
        "Nrvnqsr Chaos",
        "Nero"
      },
      {
        "Powered Ciel",
        "PCiel"
      },
      {
        "Red Arcueid",
        "Warc"
      },
      {
        "Riesbyfe Stridberg",
        "Riesbyfe"
      },
      {
        "Satsuki Yumizuka",
        "Satsuki"
      },
      {
        "Seifuku Akiha",
        "Seifuku"
      },
      {
        "Shiki Nanaya",
        "Nanaya"
      },
      {
        "Shiki Ryougi",
        "Ryougi"
      },
      {
        "Shiki Tohno",
        "Tohno"
      },
      {
        "Sion Eltnam Atlasia",
        "Sion"
      },
      {
        "Sion Tatari",
        "VSion"
      },
      {
        "Wallachia",
        "Warachia"
      },
      {
        "White Len",
        "White Len"
      }
    };


        public static List<string> Moons = new List<string>()
        {
      "Crescent",
      "Full",
      "Half"
    };

        public Match CurrentMatch;
        private CharacterCutIn CutIn1 = new CharacterCutIn(new BitmapImage(new Uri("cutins/Sion Eltnam Atlasia.png", UriKind.Relative)), new BitmapImage(new Uri("moons/Crescent.png", UriKind.Relative)));
        private CharacterCutIn CutIn2 = new CharacterCutIn(new BitmapImage(new Uri("cutins/Sion Tatari.png", UriKind.Relative)), new BitmapImage(new Uri("moons/Crescent.png", UriKind.Relative)));
        private MeltyHook.MeltyBlood hook = new MeltyHook.MeltyBlood();
        private bool isLooping = false;


        public string Player1
        {
            get => this.txtPlayer1.Text;
            set => this.txtPlayer1.Text = value;
        }

        public string Player2
        {
            get => this.txtPlayer2.Text;
            set => this.txtPlayer2.Text = value;
        }

        public string Character1 => this.cmbMoon1.Text[0].ToString() + "-" + this.Shorthands[this.cmbChar1.Text];

        public string Character2 => this.cmbMoon2.Text[0].ToString() + "-" + this.Shorthands[this.cmbChar2.Text];

        public string Round
        {
            get => this.txtRound.Text;
            set => this.txtRound.Text = value;
        }

        public string Tournament
        {
            get => this.txtTournament.Text;
            set => this.txtTournament.Text = value;
        }

        public MainWindow()
        {
            this.InitializeComponent();
            this.cmbChar1.ItemsSource = (IEnumerable)MainWindow.CharacterList;
            this.cmbChar2.ItemsSource = (IEnumerable)MainWindow.CharacterList;
            this.cmbMoon1.ItemsSource = (IEnumerable)MainWindow.Moons;
            this.cmbMoon2.ItemsSource = (IEnumerable)MainWindow.Moons;
            this.CutIn1.Title = "Player 1 Cut-In";
            this.CutIn2.Title = "Player 2 Cut-In";

            #region Filling the CharacterArray
            CharacterArray[0x00] = "Sion Eltnam Atlasia";
            CharacterArray[0x01] = "Arcueid Brunestud";
            CharacterArray[0x02] = "Ciel";
            CharacterArray[0x03] = "Akiha Tohno";
            CharacterArray[0x04] = "Hisui & Kohaku";
            CharacterArray[0x05] = "Hisui";
            CharacterArray[0x06] = "Kohaku";
            CharacterArray[0x07] = "Shiki Tohno";
            CharacterArray[0x08] = "Miyako Arima";
            CharacterArray[0x09] = "Wallachia";
            CharacterArray[0x0A] = "Nrvnqsr Chaos";
            CharacterArray[0x0B] = "Sion Tatari";
            CharacterArray[0x0C] = "Red Arcueid";
            CharacterArray[0x0D] = "Akiha Vermillion";
            CharacterArray[0x0E] = "Mech-Hisui";
            CharacterArray[0x0F] = "Shiki Nanaya";
            CharacterArray[0x11] = "Satsuki Yumizuka";
            CharacterArray[0x12] = "Len";
            CharacterArray[0x13] = "Powered Ciel";
            CharacterArray[0x14] = "Neco-Arc";
            CharacterArray[0x16] = "Aoko Aozaki";
            CharacterArray[0x17] = "White Len";
            CharacterArray[0x19] = "Neco-Arc Chaos";
            CharacterArray[0x1C] = "Kouma Kishima";
            CharacterArray[0x1D] = "Seifuku Akiha";
            CharacterArray[0x1E] = "Riesbyfe Stridberg";
            CharacterArray[0x1F] = "Michael Roa Valdamjong";
            CharacterArray[0x21] = "Shiki Ryougi";
            CharacterArray[0x22] = "Neco & Mech";
            CharacterArray[0x23] = "Koha & Mech";
            CharacterArray[0x33] = "Archetype-Earth";
            CharacterArray[0x63] = "";
            #endregion
        }

        private void btnSwap_Click(object sender, RoutedEventArgs e)
        {
            object selectedItem1 = this.cmbChar1.SelectedItem;
            this.cmbChar1.SelectedItem = this.cmbChar2.SelectedItem;
            this.cmbChar2.SelectedItem = selectedItem1;
            string text1 = this.txtPlayer1.Text;
            this.txtPlayer1.Text = this.txtPlayer2.Text;
            this.txtPlayer2.Text = text1;
            object selectedItem2 = this.cmbMoon1.SelectedItem;
            this.cmbMoon1.SelectedItem = this.cmbMoon2.SelectedItem;
            this.cmbMoon2.SelectedItem = selectedItem2;
            string text2 = this.txtScore1.Text;
            this.txtScore1.Text = this.txtScore2.Text;
            this.txtScore2.Text = text2;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText("./player1.txt", this.txtPlayer1.Text);
            File.WriteAllText("./player2.txt", this.txtPlayer2.Text);
            File.WriteAllText("./commentary.txt", this.txtCommentators.Text);
            File.WriteAllText("./score1.txt", this.txtScore1.Text);
            File.WriteAllText("./score2.txt", this.txtScore2.Text);
            File.WriteAllText("./round.txt", this.txtRound.Text);
            File.WriteAllText("./tournament.txt", this.txtTournament.Text);
            if (this.cmbChar1.Text != "")
                this.CutIn1.ChangeSource(new BitmapImage(new Uri("cutins/" + this.cmbChar1.Text + ".png", UriKind.Relative)), new BitmapImage(new Uri("moons/" + this.cmbMoon1.Text + ".png", UriKind.Relative)));
            if (this.cmbChar2.Text != "")
                this.CutIn2.ChangeSource(new BitmapImage(new Uri("cutins/" + this.cmbChar2.Text + ".png", UriKind.Relative)), new BitmapImage(new Uri("moons/" + this.cmbMoon2.Text + ".png", UriKind.Relative)));
        }

        private void btnUpdateStreamed_Click(object sender, RoutedEventArgs e)
        {
            if (!isLooping)
              {
                isLooping = true;
                Task.Factory.StartNew(() =>
                {
                    while (isLooping)
                    {
                        // Verify if Melty is there 
                        if (!hook.SearchForMelty())
                            hook.GetMB();

                        System.Threading.Thread.Sleep(1000);
                    }
                });

                Task.Factory.StartNew(() =>
                {
                    while (isLooping)
                    {

                        if (hook.SearchForMelty())
                        {
                            string char1 = CharacterArray[hook.ReadMem((int)MeltyHook.MeltyMem.CC_P1_CHARACTER_ADDR, 1)[0]];
                            string char2 = CharacterArray[hook.ReadMem((int)MeltyHook.MeltyMem.CC_P2_CHARACTER_ADDR, 1)[0]];
                            string moon1 = Moons[hook.ReadMem((int)MeltyHook.MeltyMem.CC_P1_MOON_SELECTOR_ADDR, 1)[0]];
                            string moon2 = Moons[hook.ReadMem((int)MeltyHook.MeltyMem.CC_P2_MOON_SELECTOR_ADDR, 1)[0]];

                            if (char1 != null && char1.Length > 1)
                            {
                                this.Dispatcher.Invoke(() => cmbChar1.Text = char1);
                            }

                            if (char2 != null && char2.Length > 1)
                            {
                                this.Dispatcher.Invoke(() => cmbChar2.Text = char2);
                            }

                            if (moon1 != null && moon1.Length > 1)
                            {
                                this.Dispatcher.Invoke(() => cmbMoon1.Text = moon1);
                            }

                            if (moon2 != null && moon2.Length > 1)
                            {
                                this.Dispatcher.Invoke(() => cmbMoon2.Text = moon2);
                            }

                            this.Dispatcher.Invoke(() => UpdateCutIns());
                        }

                        System.Threading.Thread.Sleep(16);
                    }
                });


            }
        }

        //private void btnUpdateStreamed_Click(object sender, RoutedEventArgs e)
        //{
        //    if (this.CurrentMatch == null)
        //        this.CurrentMatch = new Match(this.Player1, this.Player2, this.Character1, this.Character2, this.Round, this.Tournament);
        //    else if (this.CurrentMatch != null && !this.CurrentMatch.IsNewMatch(this.Player1, this.Player2))
        //    {
        //        if (this.CurrentMatch.Player1 == this.Player1)
        //        {
        //            if (!this.CurrentMatch.Characters1.Contains(this.Character1))
        //                this.CurrentMatch.Characters1.Add(this.Character1);
        //            if (this.CurrentMatch.Characters2.Contains(this.Character2))
        //                return;
        //            this.CurrentMatch.Characters2.Add(this.Character2);
        //        }
        //        else
        //        {
        //            if (!this.CurrentMatch.Characters1.Contains(this.Character2))
        //                this.CurrentMatch.Characters1.Add(this.Character2);
        //            if (this.CurrentMatch.Characters2.Contains(this.Character1))
        //                return;
        //            this.CurrentMatch.Characters2.Add(this.Character1);
        //        }
        //    }
        //    else
        //    {
        //        File.AppendAllText("./" + this.CurrentMatch.Tournament + ".txt", this.CurrentMatch.ToString());
        //        this.CurrentMatch = new Match(this.Player1, this.Player2, this.Character1, this.Character2, this.Round, this.Tournament);
        //    }
        //}

        private void BtnImage1_Click(object sender, RoutedEventArgs e)
        {
            if (this.cmbChar1.Text != "")
                this.CutIn1.ChangeSource(new BitmapImage(new Uri("cutins/" + this.cmbChar1.Text + ".png", UriKind.Relative)), new BitmapImage(new Uri("moons/" + this.cmbMoon1.Text + ".png", UriKind.Relative)));
            if (this.CutIn1.IsVisible)
                return;
            this.CutIn1.Show();
        }

        private void BtnImage2_Click(object sender, RoutedEventArgs e)
        {
            if (this.cmbChar2.Text != "")
                this.CutIn2.ChangeSource(new BitmapImage(new Uri("cutins/" + this.cmbChar2.Text + ".png", UriKind.Relative)), new BitmapImage(new Uri("moons/" + this.cmbMoon2.Text + ".png", UriKind.Relative)));
            if (this.CutIn2.IsVisible)
                return;
            this.CutIn2.Show();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.IsClosing = true;
            this.CutIn1.Close();
            this.CutIn2.Close();

        }

        private void UpdateCutIns()
        {
            if (this.cmbChar1.Text != "")
                this.CutIn1.ChangeSource(new BitmapImage(new Uri("cutins/" + this.cmbChar1.Text + ".png", UriKind.Relative)), new BitmapImage(new Uri("moons/" + this.cmbMoon1.Text + ".png", UriKind.Relative)));
            if (this.cmbChar2.Text != "")
                this.CutIn2.ChangeSource(new BitmapImage(new Uri("cutins/" + this.cmbChar2.Text + ".png", UriKind.Relative)), new BitmapImage(new Uri("moons/" + this.cmbMoon2.Text + ".png", UriKind.Relative)));
        }
    }
}
