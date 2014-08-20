using System;
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
using System.Windows.Shapes;

using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Psalm_96
{
    /// <summary>
    /// Interaction logic for winNewSong.xaml
    /// </summary>
    public partial class winNewSong : MetroWindow
    {
        public winNewSong()
        {
            InitializeComponent();

            txtSongName.Focus();
        }

        public string SongName
        {
            get { return txtSongName.Text; }
        }

        /// <summary>
        /// Check song name input is valid and close the form
        /// </summary>
        private void CheckAndFinish()
        {
            if (string.IsNullOrWhiteSpace(txtSongName.Text))
            {
                MessageBox.Show("Please enter a name for new song!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                txtSongName.Focus();
                txtSongName.SelectAll();
            }
            else if (System.IO.File.Exists(System.IO.Path.Combine(Common.DATA_DIR, txtSongName.Text + Common.DATA_EXTS)))
            {
                MessageBox.Show("This song is already existed!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                txtSongName.Focus();
                txtSongName.SelectAll();
            }
            else
            {
                DialogResult = true;
            }
        }

        private void btnCreateSong_Click(object sender, RoutedEventArgs e)
        {
            CheckAndFinish();
        }

        private void txtSongName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CheckAndFinish();
                e.Handled = true;
            }
        }

        private void MetroWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
                e.Handled = true;
            }
        }
    }
}
