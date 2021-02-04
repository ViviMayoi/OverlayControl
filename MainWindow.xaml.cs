using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using MeltyHook;

namespace OverlayControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IComponentConnector
    {
        #region Properties
        public static bool IsClosing = false;
        public static List<string> CharacterList = new List<string>()
    {
      "_null",
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
      "Half",
      "_null"
    };
        public Match CurrentMatch;
        #endregion

        #region Private variables
        private readonly CharacterCutIn CutIn1 = new CharacterCutIn(new BitmapImage(new Uri("cutins/_null.png", UriKind.Relative)), new BitmapImage(new Uri("moons/_null.png", UriKind.Relative)));
        private readonly CharacterCutIn CutIn2 = new CharacterCutIn(new BitmapImage(new Uri("cutins/_null.png", UriKind.Relative)), new BitmapImage(new Uri("moons/_null.png", UriKind.Relative)));
        private readonly MeltyBlood hook = new MeltyBlood();
        private bool isLooping = false;

        private int scoreTotal1 = 0;
        private int scoreTotal2 = 0;
        private int scoreCurrent1 = 0;
        private int scoreCurrent2 = 0;
        #endregion

        #region String properties

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
        #endregion

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
            CharacterArray[0x63] = "_null";
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
            File.WriteAllText("./round.txt", this.txtRound.Text);
            UpdateScores();
            File.WriteAllText("./tournament.txt", this.txtTournament.Text);
            UpdateCutIns();
        }

        private void btnHookToMelty_Click(object sender, RoutedEventArgs e)
        {
            
            isLooping = !isLooping;
            if (isLooping)
            {
                // Update button description
                btnHookToMelty.Content = "Unhook from MBAACC";
                btnHookToMelty.FontSize = 11;

                Task.Factory.StartNew(() =>
                {
                    while (isLooping)
                    {
                        // Verify if Melty is there 
                        if (!hook.SearchForMelty())
                        {
                            // If no Melty is found, clear the cut-ins and scores
                            if (!hook.GetMB())
                            {
                                this.Dispatcher.Invoke(() => cmbChar1.Text = "_null");
                                this.Dispatcher.Invoke(() => cmbChar2.Text = "_null");
                                this.Dispatcher.Invoke(() => cmbMoon1.Text = "_null");
                                this.Dispatcher.Invoke(() => cmbMoon2.Text = "_null");
                                this.Dispatcher.Invoke(() => UpdateCutIns());
                                scoreCurrent1 = 0;
                                scoreCurrent2 = 0;
                                scoreTotal1 = 0;
                                scoreTotal2 = 0;
                            }
                            // If a Melty is found, set the scores visually back to 0
                            else
                            {
                                this.Dispatcher.Invoke(() => txtScore1.Text = "0");
                                this.Dispatcher.Invoke(() => txtScore2.Text = "0");
                                this.Dispatcher.Invoke(() => UpdateScores());
                            }
                        }

                        // Loop every second
                        System.Threading.Thread.Sleep(1000);
                    }
                });

                Task.Factory.StartNew(() =>
                {
                    while (isLooping)
                    {
                        if (hook.SearchForMelty())
                        {
                            // Read from Melty's memory
                            bool select1 = hook.ReadMem((int)MeltyMem.CC_P1_SELECTOR_MODE_ADDR, 1)[0] >= 1;
                            bool select2 = hook.ReadMem((int)MeltyMem.CC_P2_SELECTOR_MODE_ADDR, 1)[0] >= 1;
                            string char1 = CharacterArray[hook.ReadMem((int)MeltyMem.CC_P1_CHARACTER_ADDR, 1)[0]];
                            string char2 = CharacterArray[hook.ReadMem((int)MeltyMem.CC_P2_CHARACTER_ADDR, 1)[0]];
                            string moon1 = Moons[hook.ReadMem((int)MeltyMem.CC_P1_MOON_SELECTOR_ADDR, 1)[0]];
                            string moon2 = Moons[hook.ReadMem((int)MeltyMem.CC_P2_MOON_SELECTOR_ADDR, 1)[0]];
                            int score1 = hook.ReadMem((int)MeltyMem.CC_P1_SCORE_ADDR, 1)[0];
                            int score2 = hook.ReadMem((int)MeltyMem.CC_P2_SCORE_ADDR, 1)[0];

                            // Update the cut-ins
                            if (char1 != null && char1.Length > 1)
                                this.Dispatcher.Invoke(() => cmbChar1.Text = char1);
                            else
                                this.Dispatcher.Invoke(() => cmbChar1.Text = "_null");

                            if (char2 != null && char2.Length > 1)
                                this.Dispatcher.Invoke(() => cmbChar2.Text = char2);
                            else
                                this.Dispatcher.Invoke(() => cmbChar2.Text = "_null");

                            if (moon1 != null && moon1.Length > 1 && select1)
                                this.Dispatcher.Invoke(() => cmbMoon1.Text = moon1);
                            else
                                this.Dispatcher.Invoke(() => cmbMoon1.Text = "_null");

                            if (moon2 != null && moon2.Length > 1 && select2)
                                this.Dispatcher.Invoke(() => cmbMoon2.Text = moon2);
                            else
                                this.Dispatcher.Invoke(() => cmbMoon2.Text = "_null");

                            this.Dispatcher.Invoke(() => UpdateCutIns());

                            // Update the scores if necessary
                            if (score1 > scoreCurrent1)
                            {
                                scoreTotal1++;
                                this.Dispatcher.Invoke(() => txtScore1.Text = scoreTotal1.ToString());
                                this.Dispatcher.Invoke(() => UpdateScores());
                            }
                            if (score2 > scoreCurrent2)
                            {
                                scoreTotal2++;
                                this.Dispatcher.Invoke(() => txtScore2.Text = scoreTotal2.ToString());
                                this.Dispatcher.Invoke(() => UpdateScores());
                            }

                            // Update the app's score counter
                            scoreCurrent1 = score1;
                            scoreCurrent2 = score2;
                        }

                        // Update once per in-game frame 
                        System.Threading.Thread.Sleep(16);
                    }
                });


            }

            else
            {
                // Update button description
                btnHookToMelty.FontSize = 12; 
                btnHookToMelty.Content = "Hook to MBAACC";
            }


        }

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

        private void UpdateScores()
        {
            File.WriteAllText("./score1.txt", this.txtScore1.Text);
            File.WriteAllText("./score2.txt", this.txtScore2.Text);
        }
    }
}
