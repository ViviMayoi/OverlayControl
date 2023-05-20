using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using MeltyHook;

namespace OverlayControl
{
    public partial class MainWindow : Window, IComponentConnector
    {
        #region Properties
        public static bool IsClosing = false;
        public static List<string> CharacterList = new List<string>();
        public static string[] CharacterArray = new string[0x64];
        public static List<string> Moons = new List<string>() { "Crescent", "Full", "Half", "Null" };
        public Match CurrentMatch;
        #endregion

        #region String properties

        public string Player1
        {
            get
            {
                // Check for sponsor tag
                if (txtSponsor1.Text.Trim() == string.Empty)
                    return txtPlayer1.Text;
                else
                    return txtSponsor1.Text + " | " + txtPlayer1.Text;
            }
            set => txtPlayer1.Text = value;
        }

        public string Player2
        {
            get
            {
                // Check for sponsor tag
                if (txtSponsor2.Text.Trim() == string.Empty)
                    return txtPlayer2.Text;
                else
                    return txtSponsor2.Text + " | " + txtPlayer2.Text;
            }
            set => txtPlayer2.Text = value;
        }

        public string Character1 => cmbMoon1.Text[0].ToString() + "-" + cmbChar1.Text;

        public string Character2 => cmbMoon2.Text[0].ToString() + "-" + cmbChar2.Text;

        public string Round
        {
            get
            {
                // Group Phase
                if (Regex.Match(txtRound.Text, @"\d+").Success)
                    return string.Concat(txtRound.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Where(w => w.Length >= 1 && char.IsLetter(w[0])).Select(w => char.ToUpper(w[0]))) + Regex.Match(txtRound.Text, @"\d+").Value;

                // Bracket Phase
                else if (Regex.Replace(txtRound.Text, @"\s+", "").EndsWith("s"))
                    return string.Concat(txtRound.Text.Replace("Semis", "Semi Finals").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Where(w => w.Length >= 1).Select(w => char.ToUpper(w[0]))) + 's';
                else
                    return string.Concat(txtRound.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Where(w => w.Length >= 1).Select(w => char.ToUpper(w[0])));
            }
            set => txtRound.Text = value;
        }

        public string Tournament
        {
            get => txtTournament.Text;
            set => txtTournament.Text = value;
        }
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

        #region Interface
        public MainWindow()
        {
            this.InitializeComponent();

            // Initialize sources
            this.cmbChar1.ItemsSource = _hook.CharacterNames[2].Where(c => c != null).OrderBy(c => c);
            this.cmbChar2.ItemsSource = _hook.CharacterNames[2].Where(c => c != null).OrderBy(c => c);
            this.cmbMoon1.ItemsSource = (IEnumerable)MainWindow.Moons;
            this.cmbMoon2.ItemsSource = (IEnumerable)MainWindow.Moons;
            this.cmbCountry1.ItemsSource = (Player.Countries[])Enum.GetValues(typeof(Player.Countries));
            this.cmbCountry2.ItemsSource = (Player.Countries[])Enum.GetValues(typeof(Player.Countries));
        }

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.IsClosing = true;
            this._visuals.Close();

        }

        #region Buttons
        private void btnImages_Click(object sender, RoutedEventArgs e)
        {
            // Update images
            if (cmbChar1.Text != "")
                _visuals.ChangeSource(new BitmapImage(new Uri("cutins/" + this.cmbChar1.Text + ".png", UriKind.Relative)), new BitmapImage(new Uri("moons/" + this.cmbMoon1.Text + ".png", UriKind.Relative)),
    new BitmapImage(new Uri("cutins/" + this.cmbChar2.Text + ".png", UriKind.Relative)), new BitmapImage(new Uri("moons/" + this.cmbMoon2.Text + ".png", UriKind.Relative)),
    new BitmapImage(new Uri("flags/" + this.cmbCountry1.Text + ".png", UriKind.Relative)), new BitmapImage(new Uri("flags/" + this.cmbCountry2.Text + ".png", UriKind.Relative)));

            // Return if images are already visible
            if (this._visuals.IsVisible)
                return;

            _visuals.Show();
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

        private void btnHookToMelty_Click(object sender, RoutedEventArgs e)
        {
            hookToMelty();
        }

        private void btnSwitchProcess_Click(object sender, RoutedEventArgs e)
        {
            _hook.SwapActiveProcess();
        }

        private void btnUpdateOverlay_Click(object sender, RoutedEventArgs e)
        {
            // Update every part of the overlay
            File.WriteAllText("./player1.txt", this.txtPlayer1.Text);
            File.WriteAllText("./player2.txt", this.txtPlayer2.Text);
            File.WriteAllText("./sponsor1.txt", this.txtSponsor1.Text);
            File.WriteAllText("./sponsor2.txt", this.txtSponsor2.Text);
            File.WriteAllText("./pronouns1.txt", this.txtPron1.Text);
            File.WriteAllText("./pronouns2.txt", this.txtPron2.Text);
            File.WriteAllText("./round.txt", this.txtRound.Text);
            File.WriteAllText("./commentary.txt", this.txtCommentary.Text);
            File.WriteAllText("./tournament.txt", this.txtTournament.Text);

            updateScores();
            updateCutIns();
        }
        #endregion
        #endregion

        #region Main methods
        private void hookToMelty()
        {
            // Change to opposite state
            _isLooping = !_isLooping;

            if (_isLooping)
            // Turning the hook loop on
            {
                // Initialize intro state
                // This variable will be set every frame to the current value found in MBAACCé
                // Possible values are 2 (during character intros), 1 (pre-round movement) and 0 (during the round proper).
                // When it turns to 1, the match has started for timestamp purposes.
                int lastIntroState = 2;

                // Update button description
                btnHookToMelty.Content = "Unhook from MBAACC";
                //btnHookToMelty.FontSize = 10;

                // Thread that voids the current stream info when the game closes
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

                // Thread that updates character, score and timestamp info
                Task.Factory.StartNew(() =>
                {
                    while (_isLooping)
                    {
                        // Verify if Melty is there 
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

                            // Check if a new match is starting
                            int currentIntroState = _hook.ReadMem((int)MeltyMem.CC_INTRO_STATE_ADDR, 1)[0];
                            if (lastIntroState != currentIntroState)
                            {
                                lastIntroState = currentIntroState;
                                if (lastIntroState == 1)
                                    this.Dispatcher.Invoke(new Action(() => manageTimestamp()));
                            }

                        }

                        // Run this once per in-game frame 
                        System.Threading.Thread.Sleep(16);
                    }
                });
            }

            else
            // Turning the hook loop off
            {
                // Update button description
                btnHookToMelty.Content = "Hook to MBAACC";
                //btnHookToMelty.FontSize = 12;
            }
        }

        private void manageTimestamp()
        {
            // Check if there is currently a match being tracked
            if (CurrentMatch != null)
            {
                // If we have a new match, save previous match to file
                if (CurrentMatch.IsNewMatch(Player1, Player2))
                {
                    // Check if file exists
                    if (!File.Exists("./timestamps_" + Tournament + ".txt"))
                        // If it doesn't, create file and save initial timestamp
                        File.WriteAllText("./timestamps_" + Tournament + ".txt", CurrentMatch.StartTime.ToBinary().ToString());

                    try
                    {
                        // Get the first match's time
                        long firstMatchBin = long.Parse(File.ReadLines("./timestamps_" + Tournament + ".txt").First());
                        DateTime firstMatchTime = DateTime.FromBinary(firstMatchBin);
                        TimeSpan timestamp = CurrentMatch.StartTime - firstMatchTime;

                        // Append current match with timestamp relative to first match of the tournament
                        if (timestamp.TotalHours >= 1)
                            File.AppendAllText("./timestamps_" + Tournament + ".txt",
                                "\n" + timestamp.ToString(@"hh\:mm\:ss") + " " + CurrentMatch.ToString());
                        else
                            File.AppendAllText("./timestamps_" + Tournament + ".txt",
                                "\n" + timestamp.ToString(@"mm\:ss") + " " + CurrentMatch.ToString());
                    }
                    catch { }

                    // Create new match once the previous one is saved
                    CurrentMatch = new Match(Player1, Player2, Character1, Character2, Round);
                }

                else
                {
                    // If not, check if either of them changed characters and update match status
                    if (CurrentMatch.IsReversed(Player1, Player2) == false)
                    {
                        if (!CurrentMatch.Characters1.Contains(Character1))
                            CurrentMatch.Characters1.Add(Character1);
                        if (!CurrentMatch.Characters2.Contains(Character2))
                            CurrentMatch.Characters2.Add(Character2);
                    }
                    else if (CurrentMatch.IsReversed(Player1, Player2) == true)
                    {
                        if (!CurrentMatch.Characters1.Contains(Character2))
                            CurrentMatch.Characters1.Add(Character2);
                        if (!CurrentMatch.Characters2.Contains(Character1))
                            CurrentMatch.Characters2.Add(Character1);
                    }
                }
            }
            // If previous match is null, go ahead and create new match
            else
                CurrentMatch = new Match(Player1, Player2, Character1, Character2, Round);
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
        #endregion
    }
}
