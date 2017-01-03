using System.Windows.Data;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using System.Windows;

namespace Psalm_96
{
    public class Common
    {
        //Define constant
        public static string VERSION = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

        public const string DATA_DIR = "data";
        public const string DATA_EXTS = ".lmt";
        public const string CONFIG_FILE = "config.lmt";
        public static Configuration appConfig;

        public const bool CURRENT_BACKGROUND = true;

        public const string VIDEO_DIR = "video";
        public static string[] VIDEO_EXTS = new string[] { ".wmv", ".mov", ".avi", ".mp4", ".mpeg" };
        public const string IMAGE_DIR = "image";
        public static string[] IMAGE_EXTS = new string[] { ".jpg", ".bmp", ".png", ".tif" };
        public const string VIDEO_NONE = "--- NONE ---";

        public const int VIDEO_SPEED = 100;
        public const double TRANSITION_SPEED = 0.3;

        public static Binding vlcBinding = new Binding("VideoSource");
        public static Binding imgBinding = new Binding("Source");

        /// <summary>
        /// Check and load configuration
        /// </summary>
        public static void LoadConfiguration()
        {
            if (File.Exists(CONFIG_FILE))
                appConfig = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(CONFIG_FILE));
            else
            {
                appConfig = new Configuration();
                appConfig.ScreenWidth = 4;
                appConfig.ScreenHeight = 3;
                appConfig.TextVerticalAlignment = VerticalAlignment.Center;
                appConfig.Bilingual = true;
                appConfig.Save();
            }
        }
    }
}
