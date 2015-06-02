using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Data;
using System.Diagnostics;
using System.Reflection;

namespace Psalm_96
{
    public class Common
    {
        //Define constant
        public static string VERSION = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

        public const string DATA_DIR = "data";
        public const string DATA_EXTS = ".lmt";

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
    }
}
