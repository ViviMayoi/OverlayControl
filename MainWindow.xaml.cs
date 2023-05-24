using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using MeltyHook;
using Microsoft.Win32;

namespace OverlayControl
{
    /// <summary>
    /// Main window for the OverlayControl application.
    /// </summary>
    public partial class MainWindow : Window, IComponentConnector
    {
        #region Properties
        public static List<string> CharacterList = new List<string>();
        public static string[] CharacterArray = new string[0x64];
        public static List<string> Moons = new List<string>() { "Crescent", "Full", "Half", "Null" };
        /// <summary>
        /// The current match being tracked by OverlayControl and meltyhook.
        /// </summary>
        public Match CurrentMatch
        {
            get => _currentMatch;
            set
            {
                _currentMatch = value;

                // Disable the Save Current Match context menu item if the current match is null
                if (value == null)
                    mnuSaveCurrentMatch.IsEnabled = false;
                // Enable the context menu item if the current match is being defined
                else
                    mnuSaveCurrentMatch.IsEnabled = true;
            }
        }
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
        private readonly VisualsWindow _visuals = new VisualsWindow(new BitmapImage(new Uri("cutins/Random.png", UriKind.Relative)), new BitmapImage(new Uri("moons/Null.png", UriKind.Relative)),
            new BitmapImage(new Uri("cutins/Random.png", UriKind.Relative)), new BitmapImage(new Uri("moons/Null.png", UriKind.Relative)),
            new BitmapImage(new Uri("flags/_null.png", UriKind.Relative)), new BitmapImage(new Uri("flags/_null.png", UriKind.Relative)));
        private readonly MeltyBlood _hook = new MeltyBlood();
        private Match _currentMatch = null;

        private bool _isLooping = false;
        private int _scoreCurrent1 = 0;
        private int _scoreCurrent2 = 0;
        #endregion

        #region Interface
        #region Window

        /// <summary>
        /// Constructor for the application's main window.
        /// </summary>
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

        /// <summary>
        /// Cancelable event invoked when attempting to close the application's main window.
        /// </summary>
        /// <param name="sender">The control that was used to invoke this event.</param>
        /// <param name="e">Provides data for the cancelable event.</param>
        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (CurrentMatch != null)
            {
                // Prompt the user to save the current match before closing
                MessageBoxResult result = MessageBox.Show("There is a currently active match! Save to timestamps before exiting?",
                    "Exit OverlayControl", System.Windows.MessageBoxButton.YesNoCancel, System.Windows.MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                    saveCurrentMatchToFile();
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }

            // Shutdown the application completely to close all of its windows
            Application.Current.Shutdown();
        }
        #endregion

        #region Buttons
        /// <summary>
        /// Event invoked when button btnShowImages is clicked.
        /// </summary>
        /// <param name="sender">The button that was pressed to invoke this event.</param>
        /// <param name="e">Contains state information and event data.</param>
        private void btnShowImages_Click(object sender, RoutedEventArgs e)
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

        /// <summary>
        /// Event invoked when button btnUpdateOverlay is clicked.
        /// </summary>
        /// <param name="sender">The button that was pressed to invoke this event.</param>
        /// <param name="e">Contains state information and event data.</param>
        private void btnUpdateOverlay_Click(object sender, RoutedEventArgs e)
        {
            // Update every part of the overlay
            File.WriteAllText("./player1.txt", txtPlayer1.Text);
            File.WriteAllText("./player2.txt", txtPlayer2.Text);
            File.WriteAllText("./player1_grands.txt", (bool)chkLosersSide1.IsChecked ? txtPlayer1.Text + " [L]" : txtPlayer1.Text);
            File.WriteAllText("./player2_grands.txt", (bool)chkLosersSide2.IsChecked ? txtPlayer2.Text + " [L]" : txtPlayer2.Text);
            File.WriteAllText("./sponsor1.txt", txtSponsor1.Text);
            File.WriteAllText("./sponsor2.txt", txtSponsor2.Text);
            File.WriteAllText("./pronouns1.txt", txtPronouns1.Text);
            File.WriteAllText("./pronouns2.txt", txtPronouns2.Text);
            File.WriteAllText("./round.txt", txtRound.Text);
            File.WriteAllText("./commentary.txt", txtCommentary.Text);
            File.WriteAllText("./tournament.txt", txtTournament.Text);
            File.WriteAllText("./matchcount.txt", cmbMatchCount.Text.Trim() + ' ' + txtMatchCount.Text);

            updateScores();
            updateVisuals();
        }

        /// <summary>
        /// Event invoked when button btnSwapPlayers is clicked.
        /// </summary>
        /// <param name="sender">The button that was pressed to invoke this event.</param>
        /// <param name="e">Contains state information and event data.</param>
        private void btnSwapPlayers_Click(object sender, RoutedEventArgs e)
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

            string pronouns = this.txtPronouns1.Text;
            this.txtPronouns1.Text = this.txtPronouns2.Text;
            this.txtPronouns2.Text = pronouns;

            object country = this.cmbCountry1.SelectedItem;
            this.cmbCountry1.SelectedItem = this.cmbCountry2.SelectedItem;
            this.cmbCountry2.SelectedItem = country;
        }

        /// <summary>
        /// Event invoked when button btnResetScores is clicked.
        /// </summary>
        /// <param name="sender">The button that was pressed to invoke this event.</param>
        /// <param name="e">Contains state information and event data.</param>
        private void btnResetScores_Click(object sender, RoutedEventArgs e)
        {
            txtScore1.Text = "0";
            txtScore2.Text = "0";
        }

        /// <summary>
        /// Event invoked when button btnHookToMBAACC is clicked.
        /// </summary>
        /// <param name="sender">The button that was pressed to invoke this event.</param>
        /// <param name="e">Contains state information and event data.</param>
        private void btnHookToMBAACC_Click(object sender, RoutedEventArgs e) => hookToMelty();

        /// <summary>
        /// Event invoked when button btnSwitchProcess is clicked.
        /// </summary>
        /// <param name="sender">The button that was pressed to invoke this event.</param>
        /// <param name="e">Contains state information and event data.</param>
        private void btnSwitchProcess_Click(object sender, RoutedEventArgs e) => _hook.SwapActiveProcess();
        #endregion

        #region Context menu items
        /// <summary>
        /// Event invoked when context menu item mnuSaveCurrentMatch is clicked.
        /// </summary>
        /// <param name="sender">The button that was pressed to invoke this event.</param>
        /// <param name="e">Contains state information and event data.</param>
        private void mnuSaveCurrentMatch_Click(object sender, RoutedEventArgs e)
        {
            if (!saveCurrentMatchToFile())
            {
                MessageBox.Show("There is no currently active match. Match cannot be saved to file.",
                    "Error trying to save match", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }

            // Set CurrentMatch to null to prevent further tracking of the same match
            CurrentMatch = null;
        }

        /// <summary>
        /// Event invoked when context menu item mnuFinalizeCurrent is clicked.
        /// </summary>
        /// <param name="sender">The button that was pressed to invoke this event.</param>
        /// <param name="e">Contains state information and event data.</param>
        private void mnuFinalizeCurrent_Click(object sender, RoutedEventArgs e)
        {
            // Prompt the user for the relevant timestamp
            PromptTimestampDialog prompt = new PromptTimestampDialog();

            if (prompt.ShowDialog() == true)
            {
                // Initialize and open save file dialog
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                    OverwritePrompt = true,
                    FileName = "timestamps_" + Tournament + "_final.txt"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    // Parse the time and save new file
                    if (TimeSpan.TryParse(prompt.GivenTime, CultureInfo.CurrentCulture, out TimeSpan parsedTime))
                        finalizeTimestamps(parsedTime, saveFileDialog.FileName);

                    // Add 0 hour marker if necessary
                    else if (TimeSpan.TryParse("0:" + prompt.GivenTime, CultureInfo.CurrentCulture, out parsedTime))
                        finalizeTimestamps(parsedTime, saveFileDialog.FileName);
                    else
                        MessageBox.Show("Error: ");
                    MessageBox.Show("The inputted time was not in a valid format. Please enter the timestamp using the \"hh:mm:ss\" format.",
                        "Error trying to read timestamp", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Event invoked when context menu item mnuBrowseTimestamps is clicked.
        /// </summary>
        /// <param name="sender">The button that was pressed to invoke this event.</param>
        /// <param name="e">Contains state information and event data.</param>
        private void mnuBrowseTimestamps_Click(object sender, RoutedEventArgs e)
        {
            // Initialize file browser
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                FileName = "timestamps_" + Tournament + ".txt"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Once a file is selected, prompt the user for the relevant timestamp
                PromptTimestampDialog prompt = new PromptTimestampDialog();

                if (prompt.ShowDialog() == true)
                {
                    // Initialize and open save file dialog
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                        OverwritePrompt = true,
                        FileName = "timestamps_" + Tournament + "_final.txt"
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        // Parse the time and save new file
                        if (TimeSpan.TryParse(prompt.GivenTime, CultureInfo.CurrentCulture, out TimeSpan parsedTime))
                            finalizeTimestamps(parsedTime, openFileDialog.FileName, saveFileDialog.FileName);
                        // Add 0 hour marker if necessary
                        else if (TimeSpan.TryParse("0:" + prompt.GivenTime, CultureInfo.CurrentCulture, out parsedTime))
                            finalizeTimestamps(parsedTime, openFileDialog.FileName, saveFileDialog.FileName);
                        else
                            MessageBox.Show("The inputted time was not in a valid format. Please enter the timestamp using the \"hh:mm:ss\" format.",
                                "Error trying to read timestamp", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    }
                }
            }
        }
        #endregion

        #region Text Boxes
        // Only allow numbers in score and match count text boxes

        /// <summary>
        /// Event invoked when a text box that should only contain numbers is typed into.
        /// </summary>
        /// <param name="sender">The text box that invoked this event.</param>
        /// <param name="e">Contains state information and event data.</param>
        private void txtNumber_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) =>
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);

        /// <summary>
        /// Event invoked when a text box that should only contain numbers is pasted into.
        /// </summary>
        /// <param name="sender">The text box that invoked this event.</param>
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
        /// Event invoked when a text box that should only contain numbers has data dragged down into it via mouse.
        /// </summary>
        /// <param name="sender">The text box that invoked this event.</param>
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
        #endregion

        #region Logic
        /// <summary>
        /// Recalculates the timestamps saved in the current tournament's timestamp file, writing them to a new file chosen by the user.
        /// </summary>
        /// <param name="vodTime">Time inputted by the user, representing the start time of the first match of a tournament.</param>
        /// <param name="saveFileName">Save file chosen by the user.</param>
        private void finalizeTimestamps(TimeSpan vodTime, string saveFileName) =>
            // Call overloaded method with default file name
            finalizeTimestamps(vodTime, "./timestamps_" + Tournament + ".txt", saveFileName);

        /// <summary>
        /// Recalculates the timestamps saved in the user's chosen timestamp file, writing them to a new file chosen by the user.
        /// </summary>
        /// <param name="vodTime">Time inputted by the user, representing the start time of the first match of a tournament.</param>
        /// <param name="openFileName"></param>Timestamp file location chosen by the user.
        /// <param name="saveFileName">Save file location chosen by the user.</param>
        private void finalizeTimestamps(TimeSpan vodTime, string openFileName, string saveFileName)
        {
            if (File.Exists(openFileName))
            {
                // Separate the lines
                List<string> lines = File.ReadLines(openFileName).ToList();

                foreach (string l in lines)
                {
                    if (l.IndexOf(' ') != -1)
                    {
                        string oldTime = l.Substring(0, l.IndexOf(' '));
                        TimeSpan parsedTime;

                        if (oldTime.Length == 5)
                            // Add 0 hour marker if necessary
                            if (TimeSpan.TryParse("0:" + oldTime, CultureInfo.CurrentCulture, out parsedTime))
                            {
                                TimeSpan newTime = parsedTime + vodTime;

                                if (newTime.TotalHours >= 1)
                                    File.AppendAllText(saveFileName, newTime.ToString(@"hh\:mm\:ss") + l.Substring(l.IndexOf(' ')) + "\n");
                                else
                                    File.AppendAllText(saveFileName, newTime.ToString(@"mm\:ss") + l.Substring(l.IndexOf(' ')) + "\n");
                            }
                            else
                                // No timestamps found in this line, write it back as is
                                File.AppendAllText(saveFileName, l + "\n");

                        else if (oldTime.Length == 8)
                            // If hours are already there, parse the time as is
                            if (TimeSpan.TryParse(oldTime, CultureInfo.CurrentCulture, out parsedTime))
                            {
                                TimeSpan newTime = parsedTime + vodTime;

                                if (newTime.TotalHours >= 1)
                                    File.AppendAllText(saveFileName, newTime.ToString(@"hh\:mm\:ss") + l.Substring(l.IndexOf(' ')) + "\n");
                                else
                                    File.AppendAllText(saveFileName, newTime.ToString(@"mm\:ss") + l.Substring(l.IndexOf(' ')) + "\n");
                            }
                            else
                                // No timestamps found in this line, write it back as is
                                File.AppendAllText(saveFileName, l + "\n");

                    }
                    else
                        // No timestamps found in this line, write it back as is
                        File.AppendAllText(saveFileName, l + "\n");
                }
            }
        }

        /// <summary>
        /// Starts the threads containing OverlayControl's automatic detection of game state by hooking to MBAA.exe.
        /// </summary>
        private void hookToMelty()
        {
            // Change to opposite state
            _isLooping = !_isLooping;

            if (_isLooping)
            // Turning the hook loop on
            {
                bool isFirstLoop = true;

                // Initialize intro state
                // This variable will be set every frame to the current value found in MBAACC
                // Possible values are 2 (during character intros), 1 (pre-round movement) and 0 (during the round proper).
                // When it turns to 1, the match has started for timestamp purposes.
                int lastIntroState = 2;

                // Update button description
                btnHookToMBAACC.Content = "Unhook from MBAACC";

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
                                Dispatcher.Invoke(() => cmbChar1.Text = "Random");
                                Dispatcher.Invoke(() => cmbChar2.Text = "Random");
                                Dispatcher.Invoke(() => cmbMoon1.Text = "Null");
                                Dispatcher.Invoke(() => cmbMoon2.Text = "Null");
                                Dispatcher.Invoke(() => updateVisuals());

                                _scoreCurrent1 = 0;
                                _scoreCurrent2 = 0;
                            }

                            // If a Melty is found, set the scores visually back to 0
                            else
                            {
                                Dispatcher.Invoke(() => txtScore1.Text = "0");
                                Dispatcher.Invoke(() => txtScore2.Text = "0");
                                Dispatcher.Invoke(() => updateScores());
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
                                Dispatcher.Invoke(() => cmbChar1.Text = char1);
                            else
                                Dispatcher.Invoke(() => cmbChar1.Text = "Null");

                            if (char2 != null && char2.Length > 1)
                                Dispatcher.Invoke(() => cmbChar2.Text = char2);
                            else
                                Dispatcher.Invoke(() => cmbChar2.Text = "Null");

                            if (moon1 != null && moon1.Length > 1 && select1)
                                Dispatcher.Invoke(() => cmbMoon1.Text = moon1);
                            else
                                Dispatcher.Invoke(() => cmbMoon1.Text = "Null");

                            if (moon2 != null && moon2.Length > 1 && select2)
                                Dispatcher.Invoke(() => cmbMoon2.Text = moon2);
                            else
                                Dispatcher.Invoke(() => cmbMoon2.Text = "Null");

                            Dispatcher.Invoke(() => updateVisuals());

                            // If this is the first loop, make sure the score is caught up to the in-game one
                            // It can be manually edited after the fact if in-game score is not accurate to the set count
                            if (isFirstLoop)
                            {
                                isFirstLoop = false;

                                Dispatcher.Invoke(() => txtScore1.Text = score1.ToString());
                                Dispatcher.Invoke(() => txtScore2.Text = score2.ToString());
                                Dispatcher.Invoke(() => updateScores());
                            }

                            else
                            {
                                // Update the scores if necessary
                                if (score1 > _scoreCurrent1)
                                {
                                    Dispatcher.Invoke(() => txtScore1.Text = (int.Parse(txtScore1.Text) + 1).ToString());
                                    Dispatcher.Invoke(() => updateScores());
                                }
                                if (score2 > _scoreCurrent2)
                                {
                                    Dispatcher.Invoke(() => txtScore2.Text = (int.Parse(txtScore2.Text) + 1).ToString());
                                    Dispatcher.Invoke(() => updateScores());
                                }
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
                                    Dispatcher.Invoke(new Action(() => manageCurrentMatch()));
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
                btnHookToMBAACC.Content = "Hook to MBAACC";
            }
        }

        /// <summary>
        /// Detects the state of the Match currently held in memory and writes it into the timestamps file if a new match has begun.
        /// </summary>
        private void manageCurrentMatch()
        {
            // Check if there is currently a match being tracked
            if (CurrentMatch != null)
            {
                // If we have a new match, save previous match to file
                if (CurrentMatch.IsNewMatch(Player1, Player2))
                {
                    saveCurrentMatchToFile();

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
            {

            }
            CurrentMatch = new Match(Player1, Player2, Character1, Character2, Round);
        }

        /// <summary>
        /// Saves the current active Match to the tournament's timestamps file.
        /// </summary>
        /// <returns>If the match was successfully saved to the timestamps file.</returns>
        private bool saveCurrentMatchToFile()
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

                return true;
            }
            // If there was an error, there is no current match. Return false.
            catch { return false; }
        }

        /// <summary>
        /// Saves the scores currently entered in the score text boxes to the relevant .txt files.
        /// </summary>
        private void updateScores()
        {
            File.WriteAllText("./score1.txt", txtScore1.Text);
            File.WriteAllText("./score2.txt", txtScore2.Text);
        }

        /// <summary>
        /// Updates the VisualsWindow window's image sources according to the characters, moon styles and country flags entered into the main window.
        /// </summary>
        private void updateVisuals()
        {
            if (this.cmbChar1.Text != "" && this.cmbChar2.Text != "")
                this._visuals.ChangeSource(new BitmapImage(new Uri("cutins/" + this.cmbChar1.Text + ".png", UriKind.Relative)), new BitmapImage(new Uri("moons/" + this.cmbMoon1.Text + ".png", UriKind.Relative)),
                    new BitmapImage(new Uri("cutins/" + this.cmbChar2.Text + ".png", UriKind.Relative)), new BitmapImage(new Uri("moons/" + this.cmbMoon2.Text + ".png", UriKind.Relative)),
                    new BitmapImage(new Uri("flags/" + this.cmbCountry1.Text + ".png", UriKind.Relative)), new BitmapImage(new Uri("flags/" + this.cmbCountry2.Text + ".png", UriKind.Relative)));
        }



        #endregion


    }
}