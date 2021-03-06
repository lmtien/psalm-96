﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MahApps.Metro.Controls;
using FastColoredTextBoxNS;
using System.IO;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Vlc.DotNet.Wpf;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Interops.Signatures.LibVlc.MediaListPlayer;
using Vlc.DotNet.Core.Medias;

namespace Psalm_96
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class winMain : MetroWindow
    {
        //global variables
        double transitionSpeed;
        string currentText;
        int currentSongIndex;
        bool SongListLock = false; //for checking selection changed in song list
        bool PlaylistLock = false; //for checking selection changed in playlist
        ObservableCollection<Song> SongList = new ObservableCollection<Song>();
        ObservableCollection<Song> Playlist = new ObservableCollection<Song>();

        VlcControl vlcPlayer;

        //init Display window
        winDisplay winDis = new winDisplay();

        //Create style for highlighting
        TextStyle highLightStyle = new TextStyle(System.Drawing.Brushes.DarkGreen, System.Drawing.Brushes.LightSalmon, System.Drawing.FontStyle.Italic);

        public winMain()
        {
            PreInitVlc();

            Common.LoadConfiguration();

            InitializeComponent();

            //Init Window's title
            this.Title = "Psalm 96 v" + Common.VERSION;

            InitTextVAlignment();

            InitVideo();

            InitImage();

            InitSongList();

            InitVlc();
        }

        /// <summary>
        /// Init text vertical alignment
        /// </summary>
        void InitTextVAlignment()
        {
            //Init text vertical alignment
            vboxText1.VerticalAlignment = Common.appConfig.TextVerticalAlignment;
            vboxText2.VerticalAlignment = Common.appConfig.TextVerticalAlignment;
            winDis.SetTextVerticalAlignment(Common.appConfig.TextVerticalAlignment);
        }

        /// <summary>
        /// Pre Init all things need for VLC
        /// </summary>
        private void PreInitVlc()
        {
            //Set libvlc.dll and libvlccore.dll directory path
            VlcContext.LibVlcDllsPath = CommonStrings.LIBVLC_DLLS_PATH_DEFAULT_VALUE_X86;
            //Set the vlc plugins directory path
            VlcContext.LibVlcPluginsPath = CommonStrings.PLUGINS_PATH_DEFAULT_VALUE_X86;

            //switch to 64-bit mode
            //MessageBox.Show(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"VideoLAN\VLC"));
            //VlcContext.LibVlcDllsPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"VideoLAN\VLC");
            //VlcContext.LibVlcPluginsPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"VideoLAN\VLC\plugins");


            //Set the startup options
            //VlcContext.StartupOptions.IgnoreConfig = true;
            //VlcContext.StartupOptions.LogOptions.LogInFile = false;
            //VlcContext.StartupOptions.LogOptions.ShowLoggerConsole = false;

            //Initialize the VlcContext
            VlcContext.Initialize();

            //create instance
            vlcPlayer = new VlcControl();
        }

        /// <summary>
        /// Init all stage for VLC
        /// </summary>
        private void InitVlc()
        {
            vlcPlayer.PlaybackMode = PlaybackModes.Repeat;
            //vlcPlayer.AudioProperties.IsMute = true;
            gridPreview.Children.Add(vlcPlayer);

            //Binding
            Common.vlcBinding.Source = vlcPlayer;
            vlcImage.SetBinding(Image.SourceProperty, Common.vlcBinding);
            winDis.DisplayVideo();
        }

        /// <summary>
        /// Init video folder, and add to combobox item
        /// </summary>
        private void InitVideo()
        {
            //create folder video
            Directory.CreateDirectory(Common.VIDEO_DIR);

            //add none first
            cbxVideo.Items.Add(Common.VIDEO_NONE);

            List<string> video = GetFiles(Common.VIDEO_DIR, Common.VIDEO_EXTS);
            foreach (string v in video)
            {
                cbxVideo.Items.Add(System.IO.Path.GetFileName(v));
            }

            cbxVideo.SelectedIndex = 0;
        }

        /// <summary>
        /// Init image folder, and add to combobox item
        /// </summary>
        private void InitImage()
        {
            //create folder video
            Directory.CreateDirectory(Common.IMAGE_DIR);

            List<string> image = GetFiles(Common.IMAGE_DIR, Common.IMAGE_EXTS);
            foreach (string img in image)
            {
                cbxVideo.Items.Add(System.IO.Path.GetFileName(img));
            }

            //Binding
            Common.imgBinding.Source = imgPreview;
            winDis.DisplayImage();
        }

        /// <summary>
        /// Init data folder, and add to List View
        /// </summary>
        private void InitSongList()
        {
            //create folder data
            Directory.CreateDirectory(Common.DATA_DIR);

            string[] data = Directory.GetFiles(Common.DATA_DIR, "*" + Common.DATA_EXTS).OrderBy(x => x).ToArray();

            foreach (string d in data)
            {
                try
                {
                    Song s = JsonConvert.DeserializeObject<Song>(File.ReadAllText(d));

                    SongList.Add(s);

                    //add current playlist
                    if (s.Playlist) Playlist.Add(s);
                }
                catch
                {
                }
            }

            lstSong.ItemsSource = SongList;
            lstPlaylist.ItemsSource = Playlist;
        }

        /// <summary>
        /// Return all files in folder, with pre-define filters in Common.cs
        /// </summary>
        private List<string> GetFiles(string path, string[] EXTS)
        {
            return Directory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly).Where(f => EXTS.Contains(System.IO.Path.GetExtension(f).ToLower())).OrderBy(x => x).ToList();
        }

        /// <summary>
        /// Auto change size of Left
        /// </summary>
        void UpdateSideLeftSize()
        {
            gridSideLeft.RowDefinitions[1].Height = new GridLength((gridPreview.ActualWidth * Common.appConfig.ScreenHeight) / Common.appConfig.ScreenWidth); //3,4 ; 9,16
        }

        private void gridSideLeft_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateSideLeftSize();
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Add // to comment a line\nAdd ## to add a blank line\nBookmark Line = Ctrl + B\nClear Word Left = Ctrl + Back\nClear Word Right = Ctrl + Del\nColumn Selection Mode = Alt + Left Mouse\nCopy = Ctrl + C\nCopy = Ctrl + Ins\nCut = Ctrl + X\nCut = Shift + Del\nDisplay To Projector = F5\nFind Dialog = Ctrl + F\nFind Next = F3\nGo Down in Column Selection Mode = Alt + Shift + Down\nGo Left in Column Selection Mode = Alt + Shift + Left\nGo Right in Column Selection Mode = Alt + Shift + Right\nGo Up in Column Selection Mode = Alt + Shift + Up\nGoTo Dialog = Ctrl + G\nLowerCase = Ctrl + Shift + U\nMove Selected Lines Down = Alt + Down\nMove Selected Lines Up = Alt + Up\nNext Bookmark = Ctrl + N\nNext Verse = F1\nNext Verse = Alt + Z\nPaste = Shift + Ins\nPrevious Bookmark = Ctrl + Shift + N\nPrevious Verse = F2\nPrevious Verse = Alt + X\nRedo = Ctrl + R\nReplace Dialog = Ctrl + H\nScroll Down = Ctrl + Down\nScroll Up = Ctrl + Up\nShow/Hide Psalm 96:1 = Alt + P\nShow/Hide Text = Alt + T\nSleep Mode = Alt + S\nUnbookmark Line = Ctrl + Shift + B\nUndo = Alt + Back\nUndo = Ctrl + Z\nUpperCase = Ctrl + U\nZoom In = Ctrl + Add\nZoom In = Ctrl + Mouse Wheel Down\nZoom In = Ctrl + Mouse Wheel Up\nZoom Normal = Ctrl + 0\nZoom Out = Ctrl + Subtract", "Help");
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Psalm 96 version " + Common.VERSION + "\nCopyright © 2014 by Tidus Le\nAll Rights Reserved\n\nFeedback: Lmtien9116@gmail.com\nVietnamese Community\nHope Church Singapore\n\nThis is a completely FREE software.\nBut you can drop me an email.\nSo that I can inform and send the latest version you when it is available.\nThank you for using this software!", "About");
        }

        private void fctbContent_SelectionChanged(object sender, EventArgs e)
        {
            int currentLine = fctbContent.Selection.Start.iLine;
            List<string> result = new List<string>();

            //clear background
            for (int loop = 0; loop < fctbContent.LinesCount; loop++)
            {
                fctbContent[loop].BackgroundBrush = System.Drawing.Brushes.White;
            }

            //search backward
            while (currentLine > 0)
            {
                if (string.IsNullOrWhiteSpace(fctbContent[currentLine].Text.Trim()) || string.IsNullOrWhiteSpace(fctbContent[currentLine-1].Text.Trim()))
                {
                    break;
                }
                else
                {
                    currentLine--;
                }
            }

            //Get string
            while (currentLine < fctbContent.LinesCount)
            {
                if (string.IsNullOrWhiteSpace(fctbContent[currentLine].Text.Trim()))
                {
                    break;
                }
                else
                {
                    if (Common.CURRENT_BACKGROUND) fctbContent[currentLine].BackgroundBrush = System.Drawing.Brushes.Khaki;

                    //check comment
                    if (fctbContent[currentLine].Text.IndexOf("##") >= 0)
                        result.Add("");
                    else if (fctbContent[currentLine].Text.IndexOf("//") < 0)
                        result.Add(fctbContent[currentLine].Text);
                    
                    currentLine++;
                }
            }

            //store text
            currentText = string.Join("\n", result);

            //check to show
            if ((tgbtnControlDisplayPsalm != null) && tgbtnControlDisplayPsalm.IsInitialized && !(bool)tgbtnControlDisplayPsalm.IsChecked) DisplayText(currentText);
        }

        /// <summary>
        /// Show text to the preview and projector
        /// </summary>
        private void DisplayText(string text)
        {
            //update for the projector
            winDis.DisplayText(text, transitionSpeed);

            //update text, do fade effect
            if (txtbPreview.Opacity == 0)
            {
                txtbPreview2.BeginAnimation(OpacityProperty, CustomAnimation.GetDouble(1, 0, transitionSpeed));
                //check Bilingual
                if (Common.appConfig.Bilingual && text.Contains('@'))
                {
                    txtbPreview.Text = "";
                    string[] str = text.Split('@');
                    txtbPreview.Inlines.Add(new Run(str[0]) { Foreground = Brushes.Yellow });
                    txtbPreview.Inlines.Add(new Run(str[1]) { Foreground = Brushes.White });
                }
                else
                    txtbPreview.Text = text;
                txtbPreview.BeginAnimation(OpacityProperty, CustomAnimation.GetDouble(0, 1, transitionSpeed));
            }
            else
            {
                txtbPreview.BeginAnimation(OpacityProperty, CustomAnimation.GetDouble(1, 0, transitionSpeed));
                //check Bilingual
                if (Common.appConfig.Bilingual && text.Contains('@'))
                {
                    txtbPreview2.Text = "";
                    string[] str = text.Split('@');
                    txtbPreview2.Inlines.Add(new Run(str[0]) { Foreground = Brushes.Yellow });
                    txtbPreview2.Inlines.Add(new Run(str[1]) { Foreground = Brushes.White });
                }
                else
                    txtbPreview2.Text = text;
                txtbPreview2.BeginAnimation(OpacityProperty, CustomAnimation.GetDouble(0, 1, transitionSpeed));
            }
        }

        private void tgbtnControlSleep_Checked(object sender, RoutedEventArgs e)
        {
            winDis.ShowBlackCover(transitionSpeed);
            recBlackCover.BeginAnimation(OpacityProperty, CustomAnimation.GetDouble(0, 1, transitionSpeed));
        }

        private void tgbtnControlSleep_Unchecked(object sender, RoutedEventArgs e)
        {
            winDis.HideBlackCover(transitionSpeed);
            recBlackCover.BeginAnimation(OpacityProperty, CustomAnimation.GetDouble(1, 0, transitionSpeed));
        }

        private void sldVideoSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblVideoSpeed.Content = ((int)Math.Round(sldVideoSpeed.Value)).ToString() + "%";
            double ratio = sldVideoSpeed.Value / 100;
            vlcPlayer.Rate = (float)ratio;

            SaveChangeEnable();
        }

        private void sldTransitionSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            transitionSpeed = Math.Round(sldTransitionSpeed.Value, 1);
            lblTransitionSpeed.Content = (transitionSpeed).ToString() + "s";

            SaveChangeEnable();
        }

        private void tgbtnControlDisplay_Checked(object sender, RoutedEventArgs e)
        {
            winDis.Show();
            winDis.SetScreenToFirstNonCurrent(System.Windows.Forms.Screen.FromControl(fctbContent));
            this.Focus();
            fctbContent.Focus();
        }

        private void tgbtnControlDisplay_Unchecked(object sender, RoutedEventArgs e)
        {
            winDis.Hide();
        }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void tgbtnControlDisplayText_Checked(object sender, RoutedEventArgs e)
        {
            winDis.ShowText(transitionSpeed);
            grdText.BeginAnimation(OpacityProperty, CustomAnimation.GetDouble(0, 1, transitionSpeed));
        }

        private void tgbtnControlDisplayText_Unchecked(object sender, RoutedEventArgs e)
        {
            winDis.HideText(transitionSpeed);
            grdText.BeginAnimation(OpacityProperty, CustomAnimation.GetDouble(1, 0, transitionSpeed));
        }

        private void tgbtnControlDisplayPsalm_Checked(object sender, RoutedEventArgs e)
        {
            string psalm = @"Sing to the Lord a new song;             
         Sing to the Lord, all the earth.   
                                                Psalm 96:1   ";
            DisplayText(psalm);
        }

        private void tgbtnControlDisplayPsalm_Unchecked(object sender, RoutedEventArgs e)
        {
            DisplayText(currentText);
        }

        /// <summary>
        /// Move cursor to previous verse
        /// </summary>
        private void PreviousVerse()
        {
            int currentLine = fctbContent.Selection.Start.iLine;
            bool flag_new_verse = false;

            //search backward
            while (currentLine > 0)
            {
                if (string.IsNullOrWhiteSpace(fctbContent[currentLine].Text.Trim()) || string.IsNullOrWhiteSpace(fctbContent[currentLine - 1].Text.Trim()))
                {
                    break;
                }
                else
                {
                    currentLine--;
                }
            }

            while (currentLine > 0)
            {
                currentLine--;
                if (string.IsNullOrWhiteSpace(fctbContent[currentLine].Text.Trim()))
                {
                    if (flag_new_verse)
                    {
                        currentLine++;
                        break;
                    }
                }
                else
                {
                    flag_new_verse = true;
                }
            }

            //move to the new position
            fctbContent.Focus();
            if (!string.IsNullOrWhiteSpace(fctbContent[currentLine].Text.Trim())) fctbContent.Selection.Start = new Place(0, currentLine);
            fctbContent.DoCaretVisible();
        }

        /// <summary>
        /// Move cursor to next verse
        /// </summary>
        private void NextVerse()
        {
            int currentLine = fctbContent.Selection.Start.iLine;

            while (currentLine < fctbContent.LinesCount - 1)
            {
                currentLine++;
                if (!string.IsNullOrWhiteSpace(fctbContent[currentLine].Text.Trim()) && string.IsNullOrWhiteSpace(fctbContent[currentLine - 1].Text.Trim()))
                {
                    break;
                }
            }

            //move to the new position
            fctbContent.Focus();
            if (!string.IsNullOrWhiteSpace(fctbContent[currentLine].Text.Trim())) fctbContent.Selection.Start = new Place(0, currentLine);
            fctbContent.DoCaretVisible();
        }

        private void btnControlUp_Click(object sender, RoutedEventArgs e)
        {
            PreviousVerse();
        }

        private void btnControlDown_Click(object sender, RoutedEventArgs e)
        {
            NextVerse();
        }

        private void MetroWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case (Key.F1):
                    NextVerse();
                    fctbContent.Focus();
                    e.Handled = true;
                    break;

                case (Key.F2):
                    PreviousVerse();
                    fctbContent.Focus();
                    e.Handled = true;
                    break;

                case (Key.F5):
                    tgbtnControlDisplay.IsChecked = !tgbtnControlDisplay.IsChecked;
                    fctbContent.Focus();
                    e.Handled = true;
                    break;

                case (Key.S):
                    if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                    {
                        SaveChange(lstSong.SelectedIndex);
                        fctbContent.Focus();
                        e.Handled = true;
                    }
                    break;
            }

            //handle ALT key press
            switch (e.SystemKey)
            {
                case (Key.Z):
                    if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
                    {
                        NextVerse();
                        fctbContent.Focus();
                        e.Handled = true;
                    }
                    break;

                case (Key.X):
                    if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
                    {
                        PreviousVerse();
                        fctbContent.Focus();
                        e.Handled = true;
                    }
                    break;

                case (Key.S):
                    if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
                    {
                        tgbtnControlSleep.IsChecked = !tgbtnControlSleep.IsChecked;
                        fctbContent.Focus();
                        fctbContent.Focus();
                        e.Handled = true;
                    }
                    break;

                case (Key.T):
                    if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
                    {
                        tgbtnControlDisplayText.IsChecked = !tgbtnControlDisplayText.IsChecked;
                        fctbContent.Focus();
                        fctbContent.Focus();
                        e.Handled = true;
                    }
                    break;

                case (Key.P):
                    if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
                    {
                        tgbtnControlDisplayPsalm.IsChecked = !tgbtnControlDisplayPsalm.IsChecked;
                        fctbContent.Focus();
                        fctbContent.Focus();
                        e.Handled = true;
                    }
                    break;

                case (Key.V):
                    if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
                    {
                        tgbtnControlLockVideo.IsChecked = !tgbtnControlLockVideo.IsChecked;
                        fctbContent.Focus();
                        e.Handled = true;
                    }
                    break;
            }
        }


        private void fctbContent_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            //clear previous highlighting
            e.ChangedRange.ClearStyle(highLightStyle);
            //highlight tags
            e.ChangedRange.SetStyle(highLightStyle, @"\/\/.*");

            SaveChangeEnable();
        }

        private void cbxVideo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string fileName = cbxVideo.SelectedValue.ToString();
            if (fileName.Equals(Common.VIDEO_NONE))
            {
                vlcPlayer.VideoSource = null;
                imgPreview.Source = null;
            }
            else
            {
                //check video
                if (Common.VIDEO_EXTS.Contains(System.IO.Path.GetExtension(fileName).ToLower()))
                {
                    imgPreview.Source = null;
                    try
                    {
                        PathMedia sr = new PathMedia(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, Common.VIDEO_DIR, fileName));
                        vlcPlayer.Media = sr;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Cannot open video file. Please try again later!\n\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else //image
                {
                    vlcPlayer.VideoSource = null;
                    imgPreview.Source = new BitmapImage(new Uri(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, Common.IMAGE_DIR, fileName)));
                }
            }

            if (!(bool)tgbtnControlLockVideo.IsChecked) SaveChangeEnable();
        }

        private void lstSong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((btnSave.IsEnabled == true) && (MessageBox.Show("Do you want to save all changes?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes))
            {
                SaveChange(currentSongIndex);
            }

            currentSongIndex = lstSong.SelectedIndex;

            if (lstSong.SelectedIndex < 0)
            {
                btnDelete.IsEnabled = false;
                btnSave.IsEnabled = false;
            }
            else
            {
                //load content
                try
                {
                    Song s = lstSong.SelectedItem as Song;
                    if (!(bool)tgbtnControlLockVideo.IsChecked) cbxVideo.SelectedItem = s.VideoName;
                    sldVideoSpeed.Value = s.VideoSpeed;
                    sldTransitionSpeed.Value = s.TransitionSpeed;
                    fctbContent.Text = s.Content;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot load song file!\n\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //enable controls
                btnDelete.IsEnabled = true;
                btnSave.IsEnabled = false;
            }

            //check everything for playlist
            var songs = lstSong.SelectedItems;
            btnPlaylistAdd.IsEnabled = false;
            btnPlaylistRemove.IsEnabled = false;

            //lock selection changed
            SongListLock = true;

            if (!PlaylistLock) lstPlaylist.SelectedIndex = -1;
            foreach (Song s in songs)
            {
                if (!s.Playlist)
                {
                    btnPlaylistAdd.IsEnabled = true;
                }
                else
                {
                    btnPlaylistRemove.IsEnabled = true;
                }

                if ((!PlaylistLock) && (Playlist.Contains(s))) (lstPlaylist.ItemContainerGenerator.ContainerFromItem(s) as ListViewItem).IsSelected = true;
            }
            
            //release selection changed
            SongListLock = false;
        }

        /// <summary>
        /// Reset all controls to the defaults
        /// </summary>
        //private void ReInitAllControls()
        //{
        //    cbxVideo.SelectedIndex = 0;
        //    sldVideoSpeed.Value = Common.VIDEO_SPEED;
        //    sldTransitionSpeed.Value = Common.TRANSITION_SPEED;

        //    tgbtnControlSleep.IsChecked = false;
        //    tgbtnControlDisplayText.IsChecked = true;
        //    tgbtnControlDisplayPsalm.IsChecked = false;

        //    fctbContent.Text = string.Empty;
        //}

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            //show overlay
            this.ShowOverlayAsync();

            winNewSong song = new winNewSong();
            if (song.ShowDialog() == true)
            {
                try
                {
                    Song s = new Song();
                    s.SongName = song.SongName;
                    s.VideoName = Common.VIDEO_NONE;
                    s.VideoSpeed = Common.VIDEO_SPEED;
                    s.TransitionSpeed = Common.TRANSITION_SPEED;
                    s.Content = string.Empty;
                    s.Playlist = false;

                    s.Save();

                    //Add to list
                    SongList.Add(s);
                    lstSong.SelectedItem = s;
                    fctbContent.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot create song file. Please try again later!\n\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            //hide overlay
            this.HideOverlayAsync();
        }

        /// <summary>
        /// Enable button Save
        /// </summary>
        private void SaveChangeEnable()
        {
            if (lstSong.SelectedIndex > -1) btnSave.IsEnabled = true;
        }

        /// <summary>
        /// Save all changed to file
        /// </summary>
        private void SaveChange(int idx)
        {
            try
            {
                Song s = lstSong.Items[idx] as Song;
                s.Content = fctbContent.Text;
                if (!(bool)tgbtnControlLockVideo.IsChecked) s.VideoName = cbxVideo.SelectedValue.ToString();
                s.VideoSpeed = sldVideoSpeed.Value;
                s.TransitionSpeed = sldTransitionSpeed.Value;

                s.Save();

                btnSave.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot save this song. Please try again later!\n\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveChange(lstSong.SelectedIndex);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this song?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                var songs = lstSong.SelectedItems;
                while (songs.Count > 0)
                {
                    Song s = songs[0] as Song;
                    try
                    {
                        //delete file
                        File.Delete(System.IO.Path.Combine(Common.DATA_DIR, s.SongName + Common.DATA_EXTS));

                        SongList.Remove(s);
                        if (Playlist.Contains(s)) Playlist.Remove(s);
                        songs.Remove(s);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Cannot delete this song. Please try again later!\n\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if ((btnSave.IsEnabled == true) && (MessageBox.Show("Do you want to save all changes?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes))
            {
                SaveChange(currentSongIndex);
            }

            // Close the context. 
            //VlcContext.CloseAll();
        }

        private void btnPlaylistAdd_Click(object sender, RoutedEventArgs e)
        {
            var songs = lstSong.SelectedItems;
            foreach (Song s in songs)
            {
                //check existed
                if (!Playlist.Contains(s))
                {
                    //change state
                    s.Playlist = true;
                    s.Save();

                    //add playlist
                    Playlist.Add(s);
                }
            }
        }

        private void btnPlaylistRemove_Click(object sender, RoutedEventArgs e)
        {
            var songs = lstSong.SelectedItems;
            foreach (Song s in songs)
            {
                //check existed
                if (Playlist.Contains(s))
                {
                    //change state
                    s.Playlist = false;
                    s.Save();

                    //add playlist
                    Playlist.Remove(s);
                }
            }
        }

        private void lstPlaylist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //check selection change in songlist
            if (SongListLock) return;

            //lock selection changed
            PlaylistLock = true;

            var songs = lstPlaylist.SelectedItems;
            lstSong.SelectedIndex = -1;
            foreach (Song s in songs)
            {
                (lstSong.ItemContainerGenerator.ContainerFromItem(s) as ListViewItem).IsSelected = true;
            }

            //release selection changed
            PlaylistLock = false;
        }

        private void btnOptions_Click(object sender, RoutedEventArgs e)
        {
            //show overlay
            this.ShowOverlayAsync();

            winConfiguration config = new winConfiguration();
            if (config.ShowDialog() == true)
            {
                InitTextVAlignment();
                UpdateSideLeftSize();
            }

            //hide overlay
            this.HideOverlayAsync();
        }
    }
}