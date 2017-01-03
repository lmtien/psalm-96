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

namespace Psalm_96
{
    /// <summary>
    /// Interaction logic for winConfiguration.xaml
    /// </summary>
    public partial class winConfiguration : MetroWindow
    {
        public winConfiguration()
        {
            InitializeComponent();

            InitState();
        }

        /// <summary>
        /// Read state
        /// </summary>
        void InitState()
        {
            Common.LoadConfiguration();

            //SCREEN SIZE
            if (Common.appConfig.ScreenWidth == 4)
                rbtnScreenSize4x3.IsChecked = true;
            else
                rbtnScreenSize16x9.IsChecked = true;

            //SCREEN V-ALIGNMENT
            if (Common.appConfig.TextVerticalAlignment == VerticalAlignment.Top)
                rbtnScreenVerticalAlignmentTop.IsChecked = true;
            else if (Common.appConfig.TextVerticalAlignment == VerticalAlignment.Center)
                rbtnScreenVerticalAlignmentCenter.IsChecked = true;
            else
                rbtnScreenVerticalAlignmentBottom.IsChecked = true;

            //LANGUAGE
            if (Common.appConfig.Bilingual == true)
                rbtnLanguageBilingual.IsChecked = true;
            else
                rbtnLanguageMonolingual.IsChecked = true;
        }

        private void btnSaveConfig_Click(object sender, RoutedEventArgs e)
        {
            //SCREEN SIZE
            if ((bool)rbtnScreenSize4x3.IsChecked)
            {
                Common.appConfig.ScreenWidth = 4;
                Common.appConfig.ScreenHeight = 3;
            }
            else
            {
                Common.appConfig.ScreenWidth = 16;
                Common.appConfig.ScreenHeight = 9;
            }

            //SCREEN V-ALIGNMENT
            if ((bool)rbtnScreenVerticalAlignmentTop.IsChecked)
                Common.appConfig.TextVerticalAlignment = VerticalAlignment.Top;
            else if ((bool)rbtnScreenVerticalAlignmentCenter.IsChecked)
                Common.appConfig.TextVerticalAlignment = VerticalAlignment.Center;
            else
                Common.appConfig.TextVerticalAlignment = VerticalAlignment.Bottom;

            //LANGUAGE
            if ((bool)rbtnLanguageBilingual.IsChecked)
                Common.appConfig.Bilingual = true;
            else
                Common.appConfig.Bilingual = false;

            //save
            Common.appConfig.Save();

            //this.Close();
            DialogResult = true;
        }
    }
}
