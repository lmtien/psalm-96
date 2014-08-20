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
using System.Windows.Forms;

namespace Psalm_96
{
    /// <summary>
    /// Interaction logic for winDisplay.xaml
    /// </summary>
    public partial class winDisplay : MetroWindow
    {
        public winDisplay()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            int screen = 1;
            this.Left = Screen.AllScreens[screen].Bounds.Left;
            this.Top = Screen.AllScreens[screen].Bounds.Top;
            this.Width = Screen.AllScreens[screen].Bounds.Width;
            this.Height = Screen.AllScreens[screen].Bounds.Height;
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        /// <summary>
        /// Show black cover for sleep mode
        /// </summary>
        public void ShowBlackCover(double transitionSpeed)
        {
            recBlackCover.BeginAnimation(OpacityProperty, CustomAnimation.GetDouble(0, 1, transitionSpeed));
        }

        /// <summary>
        /// Hide black cover for normal mode
        /// </summary>
        public void HideBlackCover(double transitionSpeed)
        {
            recBlackCover.BeginAnimation(OpacityProperty, CustomAnimation.GetDouble(1, 0, transitionSpeed));
        }

        /// <summary>
        /// Show Text
        /// </summary>
        public void ShowText(double transitionSpeed)
        {
            grdText.BeginAnimation(OpacityProperty, CustomAnimation.GetDouble(0, 1, transitionSpeed));
        }

        /// <summary>
        /// Hide text
        /// </summary>
        public void HideText(double transitionSpeed)
        {
            grdText.BeginAnimation(OpacityProperty, CustomAnimation.GetDouble(1, 0, transitionSpeed));
        }

        /// <summary>
        /// Show text to the preview and projector
        /// </summary>
        public void DisplayText(string text, double transitionSpeed)
        {
            //update text, do fade effect
            if (txtbText.Opacity == 0)
            {
                txtbText2.BeginAnimation(OpacityProperty, CustomAnimation.GetDouble(1, 0, transitionSpeed));
                txtbText.Text = text;
                txtbText.BeginAnimation(OpacityProperty, CustomAnimation.GetDouble(0, 1, transitionSpeed));
            }
            else
            {
                txtbText.BeginAnimation(OpacityProperty, CustomAnimation.GetDouble(1, 0, transitionSpeed));
                txtbText2.Text = text;
                txtbText2.BeginAnimation(OpacityProperty, CustomAnimation.GetDouble(0, 1, transitionSpeed));
            }
        }

        /// <summary>
        /// Display video from source
        /// </summary>
        public void DisplayVideo()
        {
            vlcImage.SetBinding(Image.SourceProperty, Common.vlcBinding);
        }
    }
}
