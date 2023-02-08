using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public static List<string> CharacterList = new List<string>();
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
      "Null"
    };
        public Match CurrentMatch;
        #endregion

        #region Private variables
        private readonly OverlayVisuals _visuals = new OverlayVisuals(new BitmapImage(new Uri("cutins/Random.png", UriKind.Relative)), new BitmapImage(new Uri("moons/Null.png", UriKind.Relative)),
            new BitmapImage(new Uri("cutins/Random.png", UriKind.Relative)), new BitmapImage(new Uri("moons/Null.png", UriKind.Relative)),
            new BitmapImage(new Uri("flags/_null.png", UriKind.Relative)), new BitmapImage(new Uri("flags/_null.png", UriKind.Relative)));
        private readonly MeltyBlood _hook = new MeltyBlood();
        private bool _isLooping = false;

        private int _scoreTotal1 = 0;
        private int _scoreTotal2 = 0;
        private int _scoreCurrent1 = 0;
        private int _scoreCurrent2 = 0;
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
            this.cmbChar1.ItemsSource = _hook.CharacterNames[2].Where(c => c != null).OrderBy(c => c);
            this.cmbChar2.ItemsSource = _hook.CharacterNames[2].Where(c => c != null).OrderBy(c => c);
            this.cmbMoon1.ItemsSource = (IEnumerable)MainWindow.Moons;
            this.cmbMoon2.ItemsSource = (IEnumerable)MainWindow.Moons;
            this.cmbCountry1.ItemsSource = (Player.Countries[])Enum.GetValues(typeof(Player.Countries));
            this.cmbCountry2.ItemsSource = (Player.Countries[])Enum.GetValues(typeof(Player.Countries));

        }

        private void btnSwap_Click(object sender, RoutedEventArgs e)
        {
            string sponsor = this.txtSponsor1.Text;
            this.txtSponsor1.Text = this.txtSponsor2.Text;
            this.txtSponsor2.Text = sponsor;

            string name = this.txtPlayer1.Text;
            this.txtPlayer1.Text = this.txtPlayer2.Text;
            this.txtPlayer2.Text = name;

            object selectedChar = this.cmbChar1.SelectedItem;
            this.cmbChar1.SelectedItem = this.cmbChar2.SelectedItem;
            this.cmbChar2.SelectedItem = selectedChar;

            object selectedMoon = this.cmbMoon1.SelectedItem;
            this.cmbMoon1.SelectedItem = this.cmbMoon2.SelectedItem;
            this.cmbMoon2.SelectedItem = selectedMoon;

            string score = this.txtScore1.Text;
            this.txtScore1.Text = this.txtScore2.Text;
            this.txtScore2.Text = score;

            string pronouns = this.txtPron1.Text;
            this.txtPron1.Text = this.txtPron2.Text;
            this.txtPron2.Text = pronouns;

            object country = this.cmbCountry1.SelectedItem;
            this.cmbCountry1.SelectedItem = this.cmbCountry2.SelectedItem;
            this.cmbCountry2.SelectedItem = country;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText("./player1.txt", this.txtPlayer1.Text);
            File.WriteAllText("./player2.txt", this.txtPlayer2.Text);
            File.WriteAllText("./sponsor1.txt", this.txtSponsor1.Text);
            File.WriteAllText("./sponsor2.txt", this.txtSponsor2.Text);
            File.WriteAllText("./pronouns1.txt", this.txtPron1.Text);
            File.WriteAllText("./pronouns2.txt", this.txtPron2.Text);
            File.WriteAllText("./commentary.txt", this.txtCommentary.Text);
            File.WriteAllText("./round.txt", this.txtRound.Text);
            updateScores();
            File.WriteAllText("./tournament.txt", this.txtTournament.Text);
            updateCutIns();
        }

        private void btnHookToMelty_Click(object sender, RoutedEventArgs e)
        {

            _isLooping = !_isLooping;
            if (_isLooping)
            {
                // Update button description
                btnHookToMelty.Content = "Unhook from MBAACC";
                btnHookToMelty.FontSize = 11;

                Task.Factory.StartNew(() =>
                {
                    while (_isLooping)
                    {
                        // Verify if Melty is there 
                        if (!_hook.SearchForMelty())
                        {
                            // If no Melty is found, clear the cut-ins and scores
                            if (!_hook.GetMB())
                            {
                                this.Dispatcher.Invoke(() => cmbChar1.Text = "Random");
                                this.Dispatcher.Invoke(() => cmbChar2.Text = "Random");
                                this.Dispatcher.Invoke(() => cmbMoon1.Text = "Null");
                                this.Dispatcher.Invoke(() => cmbMoon2.Text = "Null");
                                this.Dispatcher.Invoke(() => updateCutIns());
                                _scoreCurrent1 = 0;
                                _scoreCurrent2 = 0;
                                _scoreTotal1 = 0;
                                _scoreTotal2 = 0;
                            }
                            // If a Melty is found, set the scores visually back to 0
                            else
                            {
                                this.Dispatcher.Invoke(() => txtScore1.Text = "0");
                                this.Dispatcher.Invoke(() => txtScore2.Text = "0");
                                this.Dispatcher.Invoke(() => updateScores());
                            }
                        }

                        // Loop every second
                        System.Threading.Thread.Sleep(1000);
                    }
                });

                Task.Factory.StartNew(() =>
                {
                    while (_isLooping)
                    {
                        if (_hook.SearchForMelty())
                        {
                            // Read from Melty's memory
                            bool select1 = _hook.ReadMem((int)MeltyMem.CC_P1_SELECTOR_MODE_ADDR, 1)[0] >= 1;
                            bool select2 = _hook.ReadMem((int)MeltyMem.CC_P2_SELECTOR_MODE_ADDR, 1)[0] >= 1;
                            string char1 = _hook.CharacterNames[(Int32)NameType.Short][_hook.ReadMem((int)MeltyMem.CC_P1_CHARACTER_ADDR, 1)[0]];
                            string char2 = _hook.CharacterNames[(Int32)NameType.Short][_hook.ReadMem((int)MeltyMem.CC_P2_CHARACTER_ADDR, 1)[0]];
                            string moon1 = Moons[_hook.ReadMem((int)MeltyMem.CC_P1_MOON_SELECTOR_ADDR, 1)[0]];
                            string moon2 = Moons[_hook.ReadMem((int)MeltyMem.CC_P2_MOON_SELECTOR_ADDR, 1)[0]];
                            int score1 = _hook.ReadMem((int)MeltyMem.CC_P1_SCORE_ADDR, 1)[0];
                            int score2 = _hook.ReadMem((int)MeltyMem.CC_P2_SCORE_ADDR, 1)[0];

                            // Update the cut-ins
                            if (char1 != null && char1.Length > 1)
                                this.Dispatcher.Invoke(() => cmbChar1.Text = char1);
                            else
                                this.Dispatcher.Invoke(() => cmbChar1.Text = "Null");

                            if (char2 != null && char2.Length > 1)
                                this.Dispatcher.Invoke(() => cmbChar2.Text = char2);
                            else
                                this.Dispatcher.Invoke(() => cmbChar2.Text = "Null");

                            if (moon1 != null && moon1.Length > 1 && select1)
                                this.Dispatcher.Invoke(() => cmbMoon1.Text = moon1);
                            else
                                this.Dispatcher.Invoke(() => cmbMoon1.Text = "Null");

                            if (moon2 != null && moon2.Length > 1 && select2)
                                this.Dispatcher.Invoke(() => cmbMoon2.Text = moon2);
                            else
                                this.Dispatcher.Invoke(() => cmbMoon2.Text = "Null");

                            this.Dispatcher.Invoke(() => updateCutIns());

                            // Update the scores if necessary
                            if (score1 > _scoreCurrent1)
                            {
                                _scoreTotal1++;
                                this.Dispatcher.Invoke(() => txtScore1.Text = _scoreTotal1.ToString());
                                this.Dispatcher.Invoke(() => updateScores());
                            }
                            if (score2 > _scoreCurrent2)
                            {
                                _scoreTotal2++;
                                this.Dispatcher.Invoke(() => txtScore2.Text = _scoreTotal2.ToString());
                                this.Dispatcher.Invoke(() => updateScores());
                            }

                            // Update the app's score counter
                            _scoreCurrent1 = score1;
                            _scoreCurrent2 = score2;
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

        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            if (this.cmbChar1.Text != "")
                this._visuals.ChangeSource(new BitmapImage(new Uri("cutins/" + this.cmbChar1.Text + ".png", UriKind.Relative)), new BitmapImage(new Uri("moons/" + this.cmbMoon1.Text + ".png", UriKind.Relative)),
    new BitmapImage(new Uri("cutins/" + this.cmbChar2.Text + ".png", UriKind.Relative)), new BitmapImage(new Uri("moons/" + this.cmbMoon2.Text + ".png", UriKind.Relative)),
    new BitmapImage(new Uri("flags/" + this.cmbCountry1.Text + ".png", UriKind.Relative)), new BitmapImage(new Uri("flags/" + this.cmbCountry2.Text + ".png", UriKind.Relative)));
            if (this._visuals.IsVisible)
                return;
            this._visuals.Show();
        }

        //private void BtnImage2_Click(object sender, RoutedEventArgs e)
        //{
        //    if (this.cmbChar2.Text != "")
        //        this.CutIn2.ChangeSource(new BitmapImage(new Uri("cutins/" + this.cmbChar2.Text + ".png", UriKind.Relative)), new BitmapImage(new Uri("moons/" + this.cmbMoon2.Text + ".png", UriKind.Relative)));
        //    if (this.CutIn2.IsVisible)
        //        return;
        //    this.CutIn2.Show();
        //}

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.IsClosing = true;
            this._visuals.Close();

        }

        private void updateCutIns()
        {
            if (this.cmbChar1.Text != "" && this.cmbChar2.Text != "")
                this._visuals.ChangeSource(new BitmapImage(new Uri("cutins/" + this.cmbChar1.Text + ".png", UriKind.Relative)), new BitmapImage(new Uri("moons/" + this.cmbMoon1.Text + ".png", UriKind.Relative)),
                    new BitmapImage(new Uri("cutins/" + this.cmbChar2.Text + ".png", UriKind.Relative)), new BitmapImage(new Uri("moons/" + this.cmbMoon2.Text + ".png", UriKind.Relative)),
                    new BitmapImage(new Uri("flags/" + this.cmbCountry1.Text + ".png", UriKind.Relative)), new BitmapImage(new Uri("flags/" + this.cmbCountry2.Text + ".png", UriKind.Relative)));
        }

        private void updateScores()
        {
            File.WriteAllText("./score1.txt", this.txtScore1.Text);
            File.WriteAllText("./score2.txt", this.txtScore2.Text);
        }

        private void btnSwitchProcess_Click(object sender, RoutedEventArgs e)
        {
            _hook.SwapActiveProcess();
        }
    }
}
